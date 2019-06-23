using CallStatisticsService.Models;
using CallStatisticsService.Services.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Linq;
using CallStatisticsService.CollectorClient;
using System.Threading;
using CallStatisticsService.Analytics;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions; 

namespace CallStatisticsService.Services.Concrete
{
    public class CallProcessingService : ICallEventsService
    {
        private const string callEventsCollectionName = "CallsEvents";

        private IConfiguration _config;
        private readonly IMongoCollection<CallEvent> _callStatsCollection;
        private ICollectorClient _callConsumer;
        private LinearRegressionService _analyticsService;

        public CallProcessingService(
            IConfiguration configuration,
            ICollectorClient callConsumer, 
            LinearRegressionService linearRegressionService)
        {
            _analyticsService = linearRegressionService;
            _callConsumer = callConsumer;
            _config = configuration;

            var client = new MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callStatsCollection = database.GetCollection<CallEvent>(callEventsCollectionName);
            _callStatsCollection = callStatsCollection;
        }

        async public Task<List<CallEvent>> GetEventsAsync()
        {
            var callStats = await _callStatsCollection.Find(x => true).ToListAsync();
            var callStatsDeserialized = callStats.Select(x =>
            {
                CallEvent callEventCopy = new CallEvent(x);
                callEventCopy.LeastCongestedHour = DeserializeBsonDoc(callEventCopy.BsonLeastCongestedHour);
                callEventCopy.MostCongestedHour = DeserializeBsonDoc(callEventCopy.BsonMostCongestedHour);
                callEventCopy.HourCongestion = DeserializeBsonDoc(callEventCopy.BsonHourCongestion);
                return callEventCopy;
            });

            return callStatsDeserialized.ToList();
        }

        async public Task<CallEvent> GetLatestEventAsync()
        {
            var latestCallEvent = await _callStatsCollection.Find(x => true).SortByDescending(x => x.LastUpdatedAt).FirstAsync();
            latestCallEvent.LeastCongestedHour = DeserializeBsonDoc(latestCallEvent.BsonLeastCongestedHour);
            latestCallEvent.MostCongestedHour = DeserializeBsonDoc(latestCallEvent.BsonMostCongestedHour);
            latestCallEvent.HourCongestion = DeserializeBsonDoc(latestCallEvent.BsonHourCongestion);
            return latestCallEvent;
        }

      
        public async void UpdateStats()
        {
            var response = _callConsumer.fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<Call> _collectedCalls = JsonConvert.DeserializeObject<List<Call>>(responseContentString);

            List<CallDuration> _collectedCallStats = ConvertCallsToStats(_collectedCalls);
            double yIntercept, slope;
            AnalyzeCallDuration(_collectedCallStats, out yIntercept, out slope);
            Dictionary<int, int> hourlyCongestion = createPredictedHourlyCongestion(yIntercept, slope);
            CallEvent callEvent = createEvent(hourlyCongestion);
            CallEvent insertedEvent = InsertNewEvent(callEvent);
            TriggerAlertEvent(callEvent);
            SendCallsTelemetry(insertedEvent);
        }

        private List<CallDuration> ConvertCallsToStats(List<Call> calls)
        {
            Func<Call, CallDuration> mapCallToUserCallStats =
                x => new CallDuration() { UserId = x.UserId, Duration = x.Duration, CallStartedHour = x.TimeStamp.Hour };

            List<CallDuration> userCallStatsList = calls.Select(mapCallToUserCallStats).ToList();
            return userCallStatsList;
        }

        private void AnalyzeCallDuration(List<CallDuration> list, out double yIntercept, out double slope)
        {
            yIntercept = 0;
            slope = 0;
            double rSquared;

            var distinctHoursCallDuration = list
                    .GroupBy(x => x.CallStartedHour)
                    .Select(x => new CallDuration() { Duration = (int)x.Average(t => t.Duration), CallStartedHour = x.Key })
                    .ToList();

            double[] hours = distinctHoursCallDuration.Select(x => (double)x.CallStartedHour).ToArray();
            double[] durations = distinctHoursCallDuration.Select(x => (double)x.Duration).ToArray();

            _analyticsService.CalculateLinearRegression(hours, durations, out rSquared, out yIntercept, out slope);
        }

        private Dictionary<int,int> createPredictedHourlyCongestion(double yIntercept, double slope)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for(int hour = 0; hour < 24; hour++)
            {
                int predictedCongestion = (int)(slope * hour + yIntercept);
                dict.Add(hour, predictedCongestion);
            }
            return dict;
        }

        private CallEvent createEvent(Dictionary<int,int> hourlyCongestion)
        {
            CallEvent callEvent = new CallEvent();
            callEvent.HourCongestion = hourlyCongestion;

            var maxHourDuration = hourlyCongestion.Values.Max();
            var keyOfMax = hourlyCongestion.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            callEvent.MostCongestedHour.Add(keyOfMax, maxHourDuration);

            var minHourDuration = hourlyCongestion.Values.Min();
            var keyOfMin = hourlyCongestion.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
            callEvent.LeastCongestedHour.Add(keyOfMin, minHourDuration);

            callEvent.LastUpdatedAt = BsonDateTime.Create(DateTime.Now);

            return callEvent;

        }

        private CallEvent InsertNewEvent(CallEvent callEvent)
        {
            CallEvent callEventSerialized = new CallEvent(callEvent);

            callEventSerialized.BsonHourCongestion = createBsonDocument(callEvent.HourCongestion);
            callEventSerialized.BsonMostCongestedHour = createBsonDocument(callEvent.MostCongestedHour);
            callEventSerialized.BsonLeastCongestedHour = createBsonDocument(callEvent.LeastCongestedHour);

            _callStatsCollection.InsertOne(callEventSerialized);
            return callEventSerialized;
        }
        private async void TriggerAlertEvent(CallEvent callEvent){
            using (HttpClient _client = new HttpClient())
            {
                Console.WriteLine("Posting to mqtt..");
                var jsonObject = JsonConvert.SerializeObject(new{alert="callsCongestionLimitPassed"});
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                await _client.PostAsync("http://172.17.0.1:34567/publish", content);

            }
        }
        private async void SendCallsTelemetry(CallEvent callEventSerialized) {
            using (HttpClient _client = new HttpClient())
            {
                Console.WriteLine("Posting telemetry to mqtt..");
                var jsonObject = JsonConvert.SerializeObject(new
                 { callsTelemetry= callEventSerialized.BsonHourCongestion});
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                await _client.PostAsync("http://172.17.0.1:34567/publish", content);

            }
        }
        


        private BsonDocument createBsonDocument(object obj)
        {
            var jsonHourCongestion = JsonConvert.SerializeObject(obj);
            var bDoc = BsonSerializer.Deserialize<BsonDocument>(jsonHourCongestion);
            return bDoc;
        }

        private Dictionary<int,int> DeserializeBsonDoc(BsonDocument doc)
        {
            var bsonDoc = BsonExtensionMethods.ToJson(doc);
            return JsonConvert.DeserializeObject<Dictionary<int, int>>(bsonDoc);
        }
    }
}