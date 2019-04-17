using SocialEvolutionSensor.Models;
using System.Collections.Generic;

namespace SocialEvolutionSensorAPI.Services
{
    public interface IMessagesDataService
    {
        List<Message> getLatest();
    }
}
