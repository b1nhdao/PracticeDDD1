namespace Mini_Ecommerce.Api.Attributes.RedisCache
{
    public class RedisCacheAttribute : Attribute
    {
        public int DurationInSeconds { get; set; } = 300;
        public string? KeyPrefix { get; set; }

        public RedisCacheAttribute() { }

        public RedisCacheAttribute(int durationInSeconds)
        {
            DurationInSeconds = durationInSeconds;
        }
    }
}
