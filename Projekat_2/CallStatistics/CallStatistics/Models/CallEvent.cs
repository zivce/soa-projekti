﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CallStatisticsService.Models
{
    public class CallEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }
        
        [BsonElement("OverLimitDuration")]
        public int OverLimitDuration { get; set; }

    }
}
