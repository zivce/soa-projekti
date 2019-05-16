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

        public CallStatsService(IConfiguration configuration)
        {
            _config = configuration;
            _endpointURL = _config.GetSection("GlobalConsts").GetSection("apiLatestCalls").Value;
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
            var client = new MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callsCollection = database.GetCollection<UserCallStats>(callStatsCollectionName);
            var callStats = await callsCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
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

        public void UpdateAllStats(List<UserCallStats> stats)
        {
            var client = new MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callsCollection = database.GetCollection<Call>(callStatsCollectionName);
            
            //Get related user stats 

            // if record already exists update total duration and return it
           

            //if doesn't insert it
        }

        private List<UserCallStats> ConvertCallsToStats(List<Call> calls)
        {
            List<UserCallStats> userCallStatsList = 
                (List<UserCallStats>) calls.Select(x => new UserCallStats() { UserId = x.UserId, TotalCallDuration = x.Duration });
            return userCallStatsList;
        }

    }

}