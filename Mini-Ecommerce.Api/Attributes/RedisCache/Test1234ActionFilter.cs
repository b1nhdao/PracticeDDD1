using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mini_Ecommerce.Api.Services.CacheService;

namespace Mini_Ecommerce.Api.Attributes.RedisCache
{
    public class Test1234ActionFilter : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveMinutes;
        private readonly string _cacheKeyPrefix;

        public Test1234ActionFilter(int timeToLiveMinutes, string cacheKeyPrefix)
        {
            _timeToLiveMinutes = timeToLiveMinutes;
            _cacheKeyPrefix = cacheKeyPrefix;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cache = context.HttpContext.RequestServices.GetService<ICacheService>();

            // Generate cache key based on route and query parameters
            var cacheKey = GenerateCacheKey(context);

            // find
            var cachedResponse = await _cache.GetAsync<object>(cacheKey);

            // hit
            if (cachedResponse != null)
            {
                // response cached value
                context.Result = new ObjectResult(cachedResponse)
                {
                    StatusCode = 200
                };
                return;
            }

            // miss
            var executedContext = await next();

            var result = executedContext.Result;

            // set
            if (executedContext.Result is ObjectResult objectResult && objectResult.StatusCode == 200)
            {
                await _cache.SetAsync(cacheKey, objectResult.Value, TimeSpan.FromMinutes(_timeToLiveMinutes));
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context)
        {
            var keyBuilder = new StringBuilder();

            // Add prefix if provided
            if (!string.IsNullOrEmpty(_cacheKeyPrefix))
            {
                keyBuilder.Append(_cacheKeyPrefix).Append(":");
            }

            // Add controller and action names
            keyBuilder.Append(context.RouteData.Values["controller"])
                      .Append(":")
                      .Append(context.RouteData.Values["action"]);

            // Add route parameters
            foreach (var param in context.ActionArguments.OrderBy(x => x.Key))
            {
                keyBuilder.Append(":").Append(param.Key).Append("=").Append(param.Value);
            }

            // Add query string parameters
            var queryString = context.HttpContext.Request.QueryString.Value;
            if (!string.IsNullOrEmpty(queryString))
            {
                keyBuilder.Append(queryString);
            }

            return keyBuilder.ToString();
        }
    }
}
