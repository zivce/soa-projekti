using SocialEvolutionSensor.Models;
using System.Collections.Generic;

namespace SocialEvolutionSensorAPI.Services
{
    public interface ISMSsDataService
    {
        List<SMS> getLatest();
    }
}
