using CallStatisticsService.Models;
using CallStatisticsService.Services.Interfaces;
using SocialEvolutionSensor.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace CallStatisticsService.Services.Concrete
{
    public class CallStatsService : ICallStatsService
    {
        private const string callStatsCollectionName = "CallsStats";

        private IConfiguration _config;
        private static string _endpointURL;
        private readonly IMongoCollection<UserCallStats> _callStatsCollection;

        public CallStatsService(IConfiguration configuration)
        {
            _config = configuration;
            _endpointURL = _config.GetSection("GlobalConsts").GetSection("apiLatestCalls").Value;
            var client = new MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callStatsCollection = database.GetCollection<UserCallStats>(callStatsCollectionName);
            _callStatsCollection = callStatsCollection;
        }


        private async Task<HttpContent> fetchData()
        {
            using (HttpClient _client = new HttpClient())
            {
                var response = await _client.GetAsync(_endpointURL);
                return response.Content;
            }
        }

        async public Task<UserCallStats> GetStats(string userId)
        {
           
            var callStats = await _callStatsCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            return callStats;
        }

        async public void UpdateStats()
        {
            var response = fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<Call> _collectedCalls = JsonConvert.DeserializeObject<List<Call>>(responseContentString);
            List<UserCallStats> _collectedCallStats = ConvertCallsToStats(_collectedCalls);
            UpdateAllStats(_collectedCallStats);
        }

        // By receiving mapped user calls from data service endpoint this will refresh 
        // the collection of user call stats or insert new call stats
        public void UpdateAllStats(List<UserCallStats> stats)
        {
            stats.ForEach(async(x) =>
            {
                var callStats = await GetStats(x.UserId);
                var callStatsExist = callStats != null;
                if(callStatsExist)
                {
                    callStats.TotalCallDuration += x.TotalCallDuration;
                    UpdateRecord(callStats);
                }
                else
                {
                    InsertRecord(x);
                }

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
            List<UserCallStats> userCallStatsList = 
                (List<UserCallStats>) calls.Select(x => new UserCallStats() { UserId = x.UserId, TotalCallDuration = x.Duration });
            return userCallStatsList;
        }

    }

}