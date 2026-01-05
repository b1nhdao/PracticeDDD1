using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mini_Ecommerce.Api.Services.CacheService;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace Mini_Ecommerce.Api.Attributes.RedisCache
{
    public class CacheRewriteAttribute : Attribute, IAsyncActionFilter
    {
        private string Prefix { get; set; } = string.Empty;
        private int Ttl { get; set; }
        private int SlidingExpiration { get; set; }
        public CacheRewriteAttribute(string prefix = "", int ttl = 30, int slidingExpiration = 0)
        {
            Prefix = prefix;
            Ttl = ttl;
            SlidingExpiration = slidingExpiration;

        }


        // Problem:
        // order có các order items. OrderItem có quantity. Product có quantity (tồn kho)
        // Khi tạo order, Product.Quantity = Quantity - OrderItem.Quantity,
        // Cách tốt nhất để clear cả các cache có liên quan như ví dụ trên ? (cả Order và cả Product)

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            var _cache = context.HttpContext.RequestServices.GetService<ICacheService>();
            var ttl = TimeSpan.FromSeconds(Ttl);
            var key = GenerateKey(context, ttl.TotalSeconds.ToString());
            var cacheResponse = await _cache.GetAsync<object>(key);
            var method = context.HttpContext.Request.Method;

            // Quẻies hopefully
            if (method == HttpMethods.Get)
            {
                //hit
                if (cacheResponse != null)
                {
                    context.Result = new OkObjectResult(cacheResponse);

                    if (SlidingExpiration != 0)
                    {
                        await _cache.SlidingExpiration(key, TimeSpan.FromSeconds(SlidingExpiration));
                    }

                    stopwatch.Stop();
                    Console.WriteLine($"Cache hit: Time taken = {stopwatch.ElapsedMilliseconds} ms");

                    return;
                }

                //miss
                var executedContext = await next();

                if (executedContext.Result is ObjectResult objectResult)
                {
                    await _cache.SetAsync(key, objectResult.Value, ttl);
                }
            }
            // Commands hopefully
            else
            {
                var executedContext = await next();
                if (executedContext.Result is ObjectResult objectResult)
                {
                    await _cache.RemoveAllWithPrefix(Prefix);
                    //await _cache.SetAsync(key, objectResult.Value, ttl);
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Cache miss: Time taken = {stopwatch.ElapsedMilliseconds} ms");
        }

        private string GenerateKey(ActionExecutingContext context, string ttl)
        {
            // key = prefix:action:hashQueryParam
            var listQueryString = context.HttpContext.Request.Query;

            var bodyString = context.HttpContext.Request.Body;

            var keybuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Prefix))
            {
                keybuilder.Append($"{Prefix}:");
            }

            keybuilder.Append($"{context.RouteData.Values["action"]}");

            var hashBuilder = new StringBuilder();
            foreach (var item in listQueryString)
            {
                hashBuilder.Append(item);
            }

            hashBuilder.Append(context.HttpContext.Request.Method);

            var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashBuilder.ToString()));
            var hashQueryParam = Convert.ToHexString(bytes)[..9].ToLower();

            keybuilder.Append($":{hashQueryParam}");
            return keybuilder.ToString();
        }
    }
}
