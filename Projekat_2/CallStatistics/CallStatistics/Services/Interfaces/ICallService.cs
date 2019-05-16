using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallStatisticsService.Services.Interfaces
{
    public interface ICallService<T>
    {
        Task<List<T>> GetDataAsync();
        void InsertData(List<T> data);
    }
}
