using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CallStatisticsService.Models
{
    public class CallDuration
    {
        public string UserId { get; set; }
        public int Duration { get; set; }
        public int CallStartedHour { get; set; }
    }
}
