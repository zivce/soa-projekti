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
    public class MessageCollectorService : IDataCollectorService<Message>
    {
        private IConfiguration _config;
        private string _endpointURL;

        public MessageCollectorService(IConfiguration configuration)
        {
            _config = configuration;
            _endpointURL = _config.GetSection("GlobalConsts").GetSection("apiMessages").Value;
        }

        public async Task<List<Message>> CollectDataAsync()
        {
            var response = fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<Message> _collectedMessages = JsonConvert.DeserializeObject<List<Message>>(responseContentString);
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
        public async Task<List<Message>> GetDataAsync()
        {
            var client = new MongoDB.Driver.MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var messageCollection = database.GetCollection<Message>("Messages");
            return await messageCollection.Find(Builders<Message>.Filter.Empty).ToListAsync();
        }

        public void PersistData(List<Message> messages)
        {
            var client = new MongoDB.Driver.MongoClient(_config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var messagesCollection = database.GetCollection<Message>("Messages");
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
