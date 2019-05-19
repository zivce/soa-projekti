using CsvHelper;
using SocialEvolutionSensor.Models;
using System.Collections.Generic;
using System.IO;

namespace SocialEvolutionSensorAPI.Services
{
    public class MessagesDataService : IMessagesDataService
    {
        private const int step = 100;
        private List<Message> MessageRecords;
        private int latestIndex;
        public MessagesDataService()
        {
            latestIndex = 0;
            MessageRecords = new List<Message>();
            using (var reader = new StreamReader("SensorData/Message.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                while (csv.Read())
                {
                    var record = csv.GetRecord<Message>();
                    MessageRecords.Add(record);
                }
            }
        }

        public List<Message> getLatest()
        {
            var records = MessageRecords.GetRange(latestIndex, step);
            latestIndex += step;
            return records;
        }

    }
}
