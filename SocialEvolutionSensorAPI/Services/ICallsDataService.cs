﻿using SocialEvolutionSensor.Models;
using System.Collections.Generic;

namespace SocialEvolutionSensorAPI.Services
{
    public interface ICallsDataService
    {
        List<Call> getLatest();
    }
}
