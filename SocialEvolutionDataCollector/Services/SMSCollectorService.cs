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
    public class SMSCollectorService : IDataCollectorService<SMS>
    {
        private IConfiguration config;
        private string _endpointURL = "http://localhost:55834/api/SMSs";

        public SMSCollectorService(IConfiguration configuration)
        {
            config = configuration;
        }

        public async Task<List<SMS>> CollectDataAsync()
        {
            var response = fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<SMS> _collectedMessages = JsonConvert.DeserializeObject<List<SMS>>(responseContentString);
            PersistData(_collectedMessages);
            return _collectedMessages;
        }

        private async Task<HttpContent> fetchData()
        {
            using (HttpClient _client = new HttpClient())
            {
                var response = await _client.GetAsync(_endpointURL);
                return response.Content;
            }
        }
        public async Task<List<SMS>> GetDataAsync()
        {
            var client = new MongoDB.Driver.MongoClient(config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var smsCollection = database.GetCollection<SMS>("Messages");
            return await smsCollection.Find(Builders<SMS>.Filter.Empty).ToListAsync();
        }

        public void PersistData(List<SMS> messages)
        {
            var client = new MongoDB.Driver.MongoClient(config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var messagesCollection = database.GetCollection<SMS>("Messages");
            try
            {
                messagesCollection.InsertMany(messages);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
