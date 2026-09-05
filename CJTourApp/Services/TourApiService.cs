using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;
using System.Net.Http.Json;

namespace CJTourApp.Services;

public class TourApiService : ITourApiService
{
    private readonly HttpClient _httpClient;

    public TourApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CJTourAPI");
    }

    public async Task<IEnumerable<TourDto>> GetToursAsync(string language = AppConstants.Culture.DefaultLanguage)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<TourDto>>($"api/tours?language={language}");
            return response ?? Enumerable.Empty<TourDto>();
        }
        catch (Exception)
        {
            return Enumerable.Empty<TourDto>();
        }
    }

    public async Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string language = AppConstants.Culture.DefaultLanguage)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<TourDto>>($"api/tours/featured?language={language}");
            return response ?? Enumerable.Empty<TourDto>();
        }
        catch (Exception)
        {
            return Enumerable.Empty<TourDto>();
        }
    }

    public async Task<TourDto?> GetTourByIdAsync(int id, string language = AppConstants.Culture.DefaultLanguage)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TourDto>($"api/tours/{id}?language={language}");
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<TourDto?> GetTourBySlugAsync(string slug, string language = AppConstants.Culture.DefaultLanguage)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TourDto>($"api/tours/slug/{slug}?language={language}");
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string language = AppConstants.Culture.DefaultLanguage)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<TourDto>>($"api/tours/category/{categoryId}?language={language}");
            return response ?? Enumerable.Empty<TourDto>();
        }
        catch (Exception)
        {
            return Enumerable.Empty<TourDto>();
        }
    }

    public async Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string language = AppConstants.Culture.DefaultLanguage)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<TourDto>>($"api/tours/destination/{destinationId}?language={language}");
            return response ?? Enumerable.Empty<TourDto>();
        }
        catch (Exception)
        {
            return Enumerable.Empty<TourDto>();
        }
    }

    public async Task<string> TestBenchmarkAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/benchmark/test");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "Test failed";
        }
        catch (Exception)
        {
            return "Error executing test";
        }
    }

}