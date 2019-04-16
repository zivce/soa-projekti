using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialEvolutionDataCollector.Services
{
    public interface IDataCollectorService<T>
    {
        Task<List<T>> CollectDataAsync();
        void PersistData();
        Task<List<T>> GetDataAsync();
    }
}