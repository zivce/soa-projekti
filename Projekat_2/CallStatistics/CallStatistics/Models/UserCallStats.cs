using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CallStatisticsService.Models
{
    public class UserCallStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("TotalCallDuration")]
        public int TotalCallDuration { get; set; }
       
    }
}
