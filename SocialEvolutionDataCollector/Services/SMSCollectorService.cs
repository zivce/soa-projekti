using Newtonsoft.Json;
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
        private static string _endpointURL = "http://localhost:55834/api/SMSs";

        public async Task<List<SMS>> CollectDataAsync()
        {
            var response = fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<SMS> _responseContent = JsonConvert.DeserializeObject<List<SMS>>(responseContentString);
            return _responseContent;
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

        public void PersistData()
        {
           
        }
    }
}
