namespace SocialEvolutionDataCollector.Services
{
    public interface IDataCollectorService
    {
        void CollectData();
        void PersistData();
        void GetData();
    }
}