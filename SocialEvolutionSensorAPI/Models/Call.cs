using System;

namespace SocialEvolutionSensor.Models
{
    public class Call
    {
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Duration { get; set; }
        public string DestUserId { get; set; }
        public string DestPhoneHash { get; set; }
    }
}
