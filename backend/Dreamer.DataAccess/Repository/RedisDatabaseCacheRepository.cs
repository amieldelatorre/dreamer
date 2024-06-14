using StackExchange.Redis;
using System.Text.Json;

namespace Dreamer.DataAccess.Repository
{
    public class RedisDatabaseCacheRepository(IDatabase redisDatabaseCache) : IDatabaseCacheRepository
    {

        public async Task SetKey<T>(string key, T value, int expireSeconds)
        {
            string jsonStringValue = JsonSerializer.Serialize(value);
            await redisDatabaseCache.StringSetAsync(key, jsonStringValue, TimeSpan.FromSeconds(expireSeconds));
        }

        public async Task<T?> GetKey<T>(string key)
        {
            string? jsonStringValue = await redisDatabaseCache.StringGetAsync(key);
            if (!string.IsNullOrWhiteSpace(jsonStringValue))
                return JsonSerializer.Deserialize<T>(jsonStringValue);

            return default;
        }

        public async Task RemoveKey(string key)
        {
            var keyExists = await redisDatabaseCache.KeyExistsAsync(key);
            if (keyExists)
                await redisDatabaseCache.KeyDeleteAsync(key);
            return;
        }
    }
}
