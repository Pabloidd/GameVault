using GameVault.Models;

namespace GameVault.Repositories
{
    public interface IPublishersRepository
    {
        Task<List<Publisher>> GetAllPublishersAsync();
        Task CreatePublisherAsync(string publisherName);
        Task UpdatePublisherAsync(string oldPublisherName, string newPublisherName);
        Task DeletePublisherAsync(string publisherName);
        Task<List<Publisher>> GetPublisherSliceAsync(int offset);
    }
}