
using StackExchange.Redis;

namespace Mini_Ecommerce.Api.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis, IDatabase db)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }

        public Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TItem>> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<IReadOnlyList<TItem>>> factory)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(params string[] cacheKeys)
        {
            throw new NotImplementedException();
        }
    }
}
