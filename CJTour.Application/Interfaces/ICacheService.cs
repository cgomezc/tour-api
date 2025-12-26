namespace CJTour.Application.Interfaces;

public interface ICacheService
{
    // TODO: Add method to get item from cache
    Task<T> GetAsync<T>(string key);
    // TODO: Add method to set item in cache
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    // TODO: Add method to remove item from cache
    Task RemoveAsync(string key);
    // TODO: Add method to clear all cache
    Task ClearAsync();
} 
