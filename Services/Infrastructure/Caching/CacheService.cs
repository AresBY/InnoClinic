using System.Text.Json;

using InnoClinic.Services.Application.Interfaces.Caching;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace InnoClinic.Services.Infrastructure.Caching
{
    public sealed class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public CacheService(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (_memoryCache.TryGetValue(key, out T? value))
            {
                return value;
            }

            try
            {
                var redisData = await _distributedCache.GetStringAsync(key, cancellationToken);
                if (redisData is not null)
                {
                    value = JsonSerializer.Deserialize<T>(redisData);
                    _memoryCache.Set(key, value, TimeSpan.FromSeconds(30));
                    return value;
                }
            }
            catch
            {
                Console.WriteLine($"[CacheService] Redis недоступен при GetAsync({key}) — используем только MemoryCache.");
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
        {
            _memoryCache.Set(key, value, TimeSpan.FromSeconds(30));

            try
            {
                var json = JsonSerializer.Serialize(value);
                await _distributedCache.SetStringAsync(
                    key,
                    json,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = ttl
                    },
                    cancellationToken);
            }
            catch
            {
                Console.WriteLine($"[CacheService] Redis недоступен при SetAsync({key}) — используем только MemoryCache.");
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _memoryCache.Remove(key);

            try
            {
                await _distributedCache.RemoveAsync(key, cancellationToken);
            }
            catch
            {
                Console.WriteLine($"[CacheService] Redis недоступен при RemoveAsync({key}) — используем только MemoryCache.");
            }
        }
    }
}
