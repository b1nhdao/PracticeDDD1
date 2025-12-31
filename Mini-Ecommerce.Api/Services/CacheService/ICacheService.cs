namespace Mini_Ecommerce.Api.Services.CacheService
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<List<string>> GetAllKeys();
        Task RemoveAllWithPrefix(string prefix);
    }
}
