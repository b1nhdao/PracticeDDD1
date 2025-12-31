using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mini_Ecommerce.Api.Services.CacheService;

namespace Mini_Ecommerce.Api.Attributes.RedisCache
{
    public class CacheRewriteActionFilter : Attribute, IAsyncActionFilter
    {
        private string Prefix { get; set; } = string.Empty;
        private int Ttl { get; set;  }

        public CacheRewriteActionFilter(string prefix = "", int ttl = 30)
        {
            Prefix = prefix;
            Ttl = ttl;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cache = context.HttpContext.RequestServices.GetService<ICacheService>();

            var ttl = TimeSpan.FromSeconds(Ttl);

            var key = GenerateKey(context, ttl.TotalSeconds.ToString());

            var cacheResponse = await _cache.GetAsync<object>(key);

            var method = context.HttpContext.Request.Method;

            if (method == "GET")
            {
                //hit
                if (cacheResponse != null)
                {
                    context.Result = new OkObjectResult(cacheResponse);
                    return;
                }

                //miss
                var executedContext = await next();

                if (executedContext.Result is ObjectResult objectResult)
                {
                    await _cache.SetAsync(key, objectResult.Value, ttl);
                }
            }
            // Not GET
            else
            {
                var executedContext = await next();
                if (executedContext.Result is ObjectResult objectResult)
                {
                    await _cache.RemoveAllWithPrefix(Prefix);
                    await _cache.SetAsync(key, objectResult.Value, ttl);
                }
            }

        }

        private string GenerateKey(ActionExecutingContext context, string ttl)
        {
            // key = prefix-method-controller-action-hash query-ttl
            var listQueryString = context.HttpContext.Request.Query;

            var bodyString = context.HttpContext.Request.Body;


            var keybuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Prefix))
            {
                keybuilder.Append($"{Prefix}-");
            }

            keybuilder.Append($"{context.RouteData.Values["controller"]}");

            var hashBuilder = new StringBuilder();
            hashBuilder.Append($"{context.HttpContext.Request.Method}-{context.RouteData.Values["action"]}");

            foreach (var item in listQueryString)
            {
                hashBuilder.Append(item);
            }

            hashBuilder.Append(context.HttpContext.Request.Method);

            var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashBuilder.ToString()));
            var hashQuery = Convert.ToHexString(bytes)[..9].ToLower();

            keybuilder.Append($"-{hashQuery}-{ttl}");
            return keybuilder.ToString();
        }
    }
}
