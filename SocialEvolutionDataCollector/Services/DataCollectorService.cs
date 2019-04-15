using System.Collections.Generic;
using System.Net.;
using System.Linq;
using SocialEvolutionDataCollector.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace SocialEvolutionDataCollector.Services
{
    public class DataCollectorService : IDataCollectorService
    {
        private readonly HttpClient _client = new HttpClient();

        public void CollectDataAsync(string url)
        {
        }

        public void GetData()
        {
            throw new System.NotImplementedException();
        }

        public void PersistData()
        {
            throw new System.NotImplementedException();
        }
    }
}