using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Mini_Ecommerce.Api.Attributes.RedisCache
{
    public class RedisCacheFilter : IAsyncActionFilter
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheFilter> _logger;

        public RedisCacheFilter(IDistributedCache cache, ILogger<RedisCacheFilter> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Lấy attribute từ action
            var cacheAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<RedisCacheAttribute>()
                .FirstOrDefault();

            if (cacheAttribute == null)
            {
                await next();
                return;
            }

            // Tạo cache key từ route và parameters
            var cacheKey = GenerateCacheKey(context, cacheAttribute.KeyPrefix);

            // Thử lấy dữ liệu từ cache
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);

                // Deserialize và trả về kết quả từ cache
                var cachedResult = JsonSerializer.Deserialize<object>(cachedData);
                context.Result = new OkObjectResult(cachedResult);
                return;
            }

            _logger.LogInformation("Cache miss for key: {CacheKey}", cacheKey);

            // Thực thi action
            var executedContext = await next();

            // Lưu kết quả vào cache nếu thành công
            if (executedContext.Result is OkObjectResult okResult && okResult.Value != null)
            {
                var serializedData = JsonSerializer.Serialize(okResult.Value);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheAttribute.DurationInSeconds)
                };

                await _cache.SetStringAsync(cacheKey, serializedData, options);
                _logger.LogInformation("Cached data for key: {CacheKey}", cacheKey);
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context, string? prefix)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            var keyBuilder = new System.Text.StringBuilder();

            if (!string.IsNullOrEmpty(prefix))
                keyBuilder.Append($"{prefix}:");

            keyBuilder.Append($"{controller}:{action}");

            // Thêm parameters vào key
            foreach (var param in context.ActionArguments.OrderBy(x => x.Key))
            {
                keyBuilder.Append($":{param.Key}={param.Value}");
            }

            return keyBuilder.ToString();
        }
    }
}
