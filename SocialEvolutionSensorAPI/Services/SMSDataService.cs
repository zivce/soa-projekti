using CsvHelper;
using SocialEvolutionSensor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SocialEvolutionSensorAPI.Services
{
    public class SMSsDataService: ISMSsDataService
    {
        private const int step = 100;
        private List<SMS> SMSRecords;
        private int latestIndex;
        public SMSsDataService()
        {
            latestIndex = 0;
            SMSRecords = new List<SMS>();
            using (var reader = new StreamReader("SensorData/SMS.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                while (csv.Read())
                {
                    var record = csv.GetRecord<SMS>();
                    SMSRecords.Add(record);
                }
            }
        }

        public List<SMS> getLatest()
        {
            var records =  SMSRecords.GetRange(latestIndex, step);
            latestIndex += step;
            return records;
        }

    }
}
