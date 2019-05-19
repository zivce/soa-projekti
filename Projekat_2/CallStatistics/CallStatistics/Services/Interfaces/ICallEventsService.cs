using CallStatisticsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallStatisticsService.Services.Interfaces
{
    public interface ICallEventsService
    {
        void UpdateStats();
        Task<List<CallEvent>> GetEventsAsync();
        Task<CallEvent> GetLatestEventAsync();
    }
}
