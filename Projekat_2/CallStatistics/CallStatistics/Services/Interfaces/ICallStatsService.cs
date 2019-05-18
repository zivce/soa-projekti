using CallStatisticsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallStatisticsService.Services.Interfaces
{
    public interface ICallStatsService
    {
        void UpdateStats();
        void UpdateAllStats(List<UserCallStats> stats);
        Task<UserCallStats> GetStats(string userId);
    }
}
