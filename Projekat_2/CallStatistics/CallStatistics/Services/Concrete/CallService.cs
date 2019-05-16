using CallStatisticsService.Services.Interfaces;
using SocialEvolutionSensor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallStatisticsService.Services.Concrete
{
    public class CallService : ICallService<Call>
    {
        public Task<List<Call>> GetDataAsync()
        {
            throw new NotImplementedException();
        }

        public void InsertData(List<Call> data)
        {
            throw new NotImplementedException();
        }
    }
}
