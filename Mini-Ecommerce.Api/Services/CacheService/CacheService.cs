
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using StackExchange.Redis;

namespace Mini_Ecommerce.Api.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var serializedValue = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            TimeSpan expiry = expiration ?? TimeSpan.FromMinutes(60);
            await _db.StringSetAsync(key, serializedValue, expiry);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task<List<string>> GetAllKeys()
        {
            var server = _redis.GetServer("redis", 6379);
            var keys = server.Keys(pattern: "*");
            return keys.Select(key => (string)key).ToList();
        }

        public async Task RemoveAllWithPrefix(string prefix)
        {
            var server = _redis.GetServer("redis", 6379);
            var keys = server.Keys(pattern: $"{prefix}*");
            foreach (var key in keys)
            {
                await _db.KeyDeleteAsync(key);
            }
        }
    }
}
