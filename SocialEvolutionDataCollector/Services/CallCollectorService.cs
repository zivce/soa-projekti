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
        private IConfiguration config;
        private static string _endpointURL = "http://localhost:55834/api/Calls";

        public CallCollectorService(IConfiguration configuration)
        {
            config = configuration;
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
            var client = new MongoDB.Driver.MongoClient(config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callsCollection = database.GetCollection<Call>("Calls");
            return await callsCollection.Find(Builders<Call>.Filter.Empty).ToListAsync();
        }

        public void PersistData(List<Call> calls)
        {
            var client = new MongoDB.Driver.MongoClient(config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var callsCollection = database.GetCollection<Call>("Calls");
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