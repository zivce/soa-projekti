using System;
using System.Collections.Generic;
using System.Text;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialEvolutionSensor.Models
{
    public class SMS
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("Incoming")]
        public Boolean Incoming { get; set; }

        [BsonElement("DestUserId")]
        public string DestUserId { get; set; }

        [BsonElement("DestPhoneHash")]
        public string DestPhoneHash { get; set; }
    }
}
