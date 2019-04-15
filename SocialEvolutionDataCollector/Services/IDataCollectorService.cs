namespace SocialEvolutionDataCollector.Services
{
    public interface IDataCollectorService
    {
        void CollectData(string url);
        void PersistData();
        void GetData();
    }
}