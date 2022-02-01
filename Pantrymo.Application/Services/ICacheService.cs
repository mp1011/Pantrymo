using Pantrymo.Application.Models;
using System.Collections.Concurrent;

namespace Pantrymo.Application.Services
{
    public interface ICacheService
    {
        Task<T?> TryGet<T>() where T : class;

        Task Add<T>(T data) where T : class;       
    }

    public class NoCacheService : ICacheService
    {
        public Task Add<T>(T data) where T : class
        {
            return Task.CompletedTask;
        }

        public Task<T?> TryGet<T>() where T : class
        {
            return Task.FromResult(null as T);
        }
    }

    public class MemoryCacheService : ICacheService
    {
        private readonly ISettingsService _settingsService;
        private ConcurrentDictionary<Type, CachedData> _cache = new ConcurrentDictionary<Type, CachedData>();

        public MemoryCacheService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public Task Add<T>(T data) where T : class
        {
            if (_cache.TryGetValue(typeof(T), out _))
                _cache.Remove(typeof(T), out _);

            var expirationTime = DateTime.Now + _settingsService.GetCacheDuration<T>();

            _cache.TryAdd(typeof(T), new CachedData(expirationTime, data));
            return Task.CompletedTask;
        }

        public Task<T?> TryGet<T>() where T:class
        {
            CachedData entry;
            _cache.TryGetValue(typeof(T), out entry);

            if(entry == null || entry.IsExpired)
            {
                _cache.Remove(typeof(T), out _);
                return Task.FromResult(null as T);
            }

            return Task.FromResult(entry.Data as T);
        }
    }
}
