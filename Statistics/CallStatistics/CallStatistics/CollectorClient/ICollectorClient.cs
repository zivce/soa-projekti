using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;


namespace CallStatisticsService.CollectorClient
{
    public interface ICollectorClient
    {
        Task<HttpContent> fetchData();
    }
}
