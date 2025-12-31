using Microsoft.AspNetCore.Mvc;
using Mini_Ecommerce.Api.Services.CacheService;

namespace Mini_Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public TestController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpPost]
        public async Task<IActionResult> Test(string key, string value)
        {
            await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(1));
            return Ok($"cached value: {key} - {value}");
        }

        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            var value = await _cacheService.GetAsync<object>(key);
            return Ok(value);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllKeys()
        {
            var result = await _cacheService.GetAllKeys();
            return Ok(result);
        }
    }
}
