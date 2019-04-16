using System;

namespace SocialEvolutionSensor.Models
{
    public class SMS
    {
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Incoming { get; set; }
        public string DestUserId { get; set; }
        public string DestPhoneHash { get; set; }
    }
}
