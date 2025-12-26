using CJTour.Application.Interfaces;

namespace CJTour.Application.Services;

public class CacheService : ICacheService
{
    private readonly Dictionary<string, object> _cache = new();

    public Task<T> GetAsync<T>(string key)
    {
        //validate the first call with no key present
        //try get value from cache
        if (_cache.TryGetValue(key, out var value)) 
        {
            return Task.FromResult((T)value);
        }
        //nullabilty of type 
        return Task.FromResult(default(T));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        _cache[key] = value;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key){
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        _cache.Clear();
        return Task.CompletedTask;
    }
}