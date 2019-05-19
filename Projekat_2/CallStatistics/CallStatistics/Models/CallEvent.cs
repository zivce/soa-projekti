using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CallStatisticsService.Models
{
    public class CallEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("LastUpdatedAt")]
        public BsonDateTime LastUpdatedAt { get; set; }
        
        // TODO change congestion into something hour -> duration related
        // it says in hour X how long the call will last Y 

        [BsonIgnore]
        [BsonElement("MostCongestedHour")]
        public Dictionary<int,int> MostCongestedHour { get; set; }

        [BsonIgnore]
        [BsonElement("LeastCongestedHour")]
        public Dictionary<int, int> LeastCongestedHour { get; set; }

        [BsonIgnore]
        [BsonElement("HourCongestion")]
        public Dictionary<int, int> HourCongestion { get; set; }

        /** Setup for dictionary serialization/deserialization **/

        [JsonIgnore]
        [BsonElement(elementName: "MostCongestedHour")]
        public BsonDocument BsonMostCongestedHour { get; set; }

        [JsonIgnore]
        [BsonElement(elementName: "LeastCongestedHour")]
        public BsonDocument BsonLeastCongestedHour { get; set; }

        [JsonIgnore]
        [BsonElement(elementName: "HourCongestion")]
        public BsonDocument BsonHourCongestion { get; set; }

        public CallEvent(CallEvent prevCallEvent)
        {
            Id = prevCallEvent.Id;
            LastUpdatedAt = prevCallEvent.LastUpdatedAt;

            MostCongestedHour = prevCallEvent.MostCongestedHour;
            LeastCongestedHour = prevCallEvent.LeastCongestedHour;
            HourCongestion = prevCallEvent.HourCongestion;

            BsonMostCongestedHour = prevCallEvent.BsonMostCongestedHour;
            BsonLeastCongestedHour = prevCallEvent.BsonLeastCongestedHour;
            BsonHourCongestion = prevCallEvent.BsonHourCongestion;
        }

        public CallEvent()
        {
            MostCongestedHour = new Dictionary<int, int>();
            LeastCongestedHour = new Dictionary<int, int>();
            HourCongestion = new Dictionary<int, int>();
        }


    }
}
