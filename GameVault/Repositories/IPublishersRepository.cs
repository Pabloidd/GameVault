using GameVault.Models;

namespace GameVault.Repositories
{
    public interface IPublishersRepository
    {
        Task<List<Publisher>> GetAllPublishersAsync();
        Task<List<Publisher>> GetPublishersByCountryAsync(string country);
        Task CreatePublisherAsync(string company, string country);
        Task UpdatePublisherAsync(string company, string newCountry);
        Task DeletePublisherAsync(string company);
        Task<List<Publisher>> GetPublishersSliceAsync(int sliceNumber);
    }
}