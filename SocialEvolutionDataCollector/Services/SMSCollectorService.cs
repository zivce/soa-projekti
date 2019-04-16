﻿using Newtonsoft.Json;
using SocialEvolutionSensor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SocialEvolutionDataCollector.Services
{
    public class SMSCollectorService : IDataCollectorService<SMS>
    {
        private string _endpointURL = "http://localhost:55834/api/SMSs";

        public async Task<List<SMS>> CollectDataAsync()
        {
            var response = fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<SMS> _collectedMessages = JsonConvert.DeserializeObject<List<SMS>>(responseContentString);
            await PersistData(_collectedMessages);
            return messages;
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
            var res = await this.CollectDataAsync();
            return res;
        }

        public void PersistData(List<SMS> messages)
        {
            var client = new MongoClient(config.GetConnectionString("socialEvolutionConnectionString"));
            var database = client.GetDatabase("socialEvolutionDb");
            var messagesCollection = database.GetCollection("Messages");
           
        }
    }
}
