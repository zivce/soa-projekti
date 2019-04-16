using CsvHelper;
using SocialEvolutionSensor.Models;
using System.Collections.Generic;
using System.IO;

namespace SocialEvolutionSensorAPI.Services
{
    public class CallsDataService : ICallsDataService
    {
        private const int step = 100;
        private List<Call> callRecords;
        private int latestIndex;
        public CallsDataService()
        {
            latestIndex = 0;
            callRecords = new List<Call>();
            using (var reader = new StreamReader("SensorData/Calls.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                while (csv.Read())
                {
                    var record = csv.GetRecord<Call>();
                    callRecords.Add(record);
                }
            }
        }

        public List<Call> getLatest()
        {
            var records = callRecords.GetRange(latestIndex, step);
            latestIndex += step;
            return records;
        }

    }
}
