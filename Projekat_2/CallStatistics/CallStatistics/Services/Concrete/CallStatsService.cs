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

namespace CallStatisticsService.Services.Concrete
{
    public class CallStatsService : ICallStatsService
    {
        private const string callStatsCollectionName = "CallsStats";

        private IConfiguration _config;
        private readonly IMongoCollection<UserCallStats> _callStatsCollection;
        private ICollectorClient _callConsumer;

        public CallStatsService(IConfiguration configuration, ICollectorClient callConsumer)
        {
            _callConsumer = callConsumer;
            _config = configuration;
            var client = new MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callStatsCollection = database.GetCollection<UserCallStats>(callStatsCollectionName);
            _callStatsCollection = callStatsCollection;
        }

        async public Task<UserCallStats> GetStats(string userId)
        {
            var callStats = await _callStatsCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            return callStats;
        }

        public async void UpdateStats()
        {
            var response = _callConsumer.fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<Call> _collectedCalls = JsonConvert.DeserializeObject<List<Call>>(responseContentString);

            List<UserCallStats> _collectedCallStats = ConvertCallsToStats(_collectedCalls);
            UpsertAllStats(_collectedCallStats);
        }

        // By receiving mapped user calls from data service endpoint this method 
        // will refresh the collection of user call stats or insert new call stats
        private void UpsertAllStats(List<UserCallStats> stats)
        {
            stats.ForEach(x =>
            {
                FindOneAndUpdateOptions<UserCallStats> options = new FindOneAndUpdateOptions<UserCallStats>();
                options.IsUpsert = true;

                UpdateDefinition<UserCallStats> incrementedTotalCallDuration = 
                Builders<UserCallStats>
                    .Update
                    .Inc(u => u.TotalCallDuration, x.TotalCallDuration);

                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;


                FilterDefinition<UserCallStats> filter =
                Builders<UserCallStats>
                    .Filter.Where(t => t.UserId == x.UserId);

                _callStatsCollection.FindOneAndUpdate(filter, incrementedTotalCallDuration, options, token);
            });
        }

        private void UpdateRecord(UserCallStats stats)
        {
            _callStatsCollection.ReplaceOne(userCallStats => userCallStats.UserId == stats.UserId, stats);
        }

        private void InsertRecord(UserCallStats stats)
        {
            _callStatsCollection.InsertOne(stats);
        }

        private List<UserCallStats> ConvertCallsToStats(List<Call> calls)
        {
            Func<Call, UserCallStats> mapCallToUserCallStats =
                x => new UserCallStats() { UserId = x.UserId, TotalCallDuration = x.Duration };

            List<UserCallStats> userCallStatsList = calls.Select(mapCallToUserCallStats).ToList();
            return userCallStatsList;
        }

    }

}