using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SocialEvolutionDataCollector.Models;
using SocialEvolutionSensor.Models;

namespace SocialEvolutionDataCollector.Services
{
    public class CallCollectorService : IDataCollectorService<Call>
    {
        
        private static string _endpointURL = "http://localhost:55834/api/Calls";

        async public Task<List<Call>> CollectDataAsync()
        {
            var response = fetchData();
            var responseContent = await response;
            var responseContentString = await responseContent.ReadAsStringAsync();
            List<Call> _responseContent = JsonConvert.DeserializeObject<List<Call>>(responseContentString);
            return _responseContent;
        }

        private async Task<HttpContent> fetchData()
        {
            using(HttpClient _client = new HttpClient())
            {
                var response = await _client.GetAsync(_endpointURL);
                return response.Content;
            }
        }

        async public Task<List<Call>> GetDataAsync()
        {
            var res = await this.CollectDataAsync();
            return res;
        }

        public void PersistData(List<Call> calls)
        {
            throw new System.NotImplementedException();
        }
    }
}