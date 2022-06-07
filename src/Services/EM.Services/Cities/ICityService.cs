namespace EM.Services.Cities
{
    public interface ICityService
    {
        Task<int> GetOrCreateCityAsync(string name);

        Task<IEnumerable<string>> GetSimilarCityNames(string name, int maxResults);
    }
}
