using GameVault.Models;
using GameVault.Options;
using Microsoft.Extensions.Options;

namespace GameVault.Repositories
{
    public class PublishersRepository : AbstractRepository, IPublishersRepository
    {
        public PublishersRepository(IOptions<MariaDbOptions> options) : base(options.Value.ConnectionString){}
        
        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            string sql = "CALL GetAllPublishers()";
            return await QueryWithConnectionAsync<Publisher>(sql);
        }
    
        public async Task CreatePublisherAsync(string publisherName)
        {
            string sql = $"CALL CreatePublisher({publisherName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task UpdatePublisherAsync(string oldPublisherName, string newPublisherName)
        {
            string sql = $"CALL UpdatePublisher({oldPublisherName}, {newPublisherName})";
            await ExecuteWithConnectionAsync(sql);
        }
    
        public async Task DeletePublisherAsync(string publisherName)
        {
            string sql = $"CALL DeletePublisher({publisherName})";
            await ExecuteWithConnectionAsync(sql);
        }

        public async Task<List<Publisher>> GetPublisherSliceAsync(int offset)
        {
            string sql = $"CALL GetPublisherSlice({offset})";
            return await QueryWithConnectionAsync<Publisher>(sql);
        }
        

    }
}