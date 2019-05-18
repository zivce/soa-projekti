using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CallStatisticsService.CollectorClient
{
    public class CallCollectorClient : ICollectorClient
    {
        private static string _endpointURL;

        public CallCollectorClient(IConfiguration configuration)
        {
            _endpointURL = configuration.GetSection("GlobalConsts").GetSection("apiLatestCalls").Value;
        }

        public async Task<HttpContent> fetchData()
        {
            using (HttpClient _client = new HttpClient())
            {
                var response = await _client.GetAsync(_endpointURL);
                return response.Content;
            }
        }
    }
}
