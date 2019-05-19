using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using SocialEvolutionSensor.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SocialEvolutionDataCollector.Services
{
    public class CallCollectorService : IDataCollectorService<Call>
    {

        private const int step = 100;
        private int latestIndex = 0;
        private const string callsCollectionName = "Calls";

        private IConfiguration _config;
        private static string _endpointURL;

        public CallCollectorService(IConfiguration configuration)
        {
            _config = configuration;
            _endpointURL = _config.GetSection("GlobalConsts").GetSection("apiCalls").Value;
        }

        async public Task<List<Call>> CollectDataAsync()
        {
            var response = fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<Call> _collectedCalls = JsonConvert.DeserializeObject<List<Call>>(responseContentString);
            PersistData(_collectedCalls);
            return _collectedCalls;
        }

        private async Task<HttpContent> fetchData()
        {
            using (HttpClient _client = new HttpClient())
            {
                var response = await _client.GetAsync(_endpointURL);
                return response.Content;
            }
        }

        async public Task<List<Call>> GetDataAsync()
        {
            var client = new MongoDB.Driver.MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callsCollection = database.GetCollection<Call>(callsCollectionName);
            return await callsCollection.Find(Builders<Call>.Filter.Empty).ToListAsync();
        }

        async public Task<List<Call>> GetLatestDataAsync()
        {
            var client = new MongoDB.Driver.MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callsCollection = database.GetCollection<Call>(callsCollectionName);
            var callListChunk = await callsCollection
                                .Find(x => true)
                                .Skip(latestIndex)
                                .Limit(step)
                                .ToListAsync();
            latestIndex += step;
            return callListChunk;
        }

        public void PersistData(List<Call> calls)
        {
            var client = new MongoDB.Driver.MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callsCollection = database.GetCollection<Call>(callsCollectionName);
            try
            {
                callsCollection.InsertMany(calls);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}