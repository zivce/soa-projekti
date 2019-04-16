using System.Collections.Generic;
using System.Linq;
using SocialEvolutionDataCollector.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace SocialEvolutionDataCollector.Services
{
    public class DataCollectorService : IDataCollectorService
    {
        var client = new MongoClient(config.GetConnectionString("dataCollectorDb"));
        var database = client.GetDatabase("BookstoreDb");

        public void CollectDataAsync(string url)
        {
        }

        public void GetData()
        {
            var calls = database.GetCollection<Call>("Calls");
            return calls;
        }

        public void PersistData()
        {
            throw new System.NotImplementedException();
        }
    }
}