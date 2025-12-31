using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Mini_Ecommerce.Api.Attributes.RedisCache
{
    public class RedisCacheFilter : Attribute, IAsyncActionFilter
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
            var cacheAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<RedisCacheAttribute>()
                .FirstOrDefault();

            if (cacheAttribute == null)
            {
                await next();
                return;
            }

            var cacheKey = GenerateCacheKey(context, cacheAttribute.KeyPrefix);

            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);

                var cachedResult = JsonSerializer.Deserialize<object>(cachedData);
                context.Result = new OkObjectResult(cachedResult);
                return;
            }

            _logger.LogInformation("Cache miss for key: {CacheKey}", cacheKey);

            var executedContext = await next();

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
