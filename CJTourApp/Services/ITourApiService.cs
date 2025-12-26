using CJTour.Contracts.DTOs;

namespace CJTourApp.Services;

public interface ITourApiService
{
    Task<IEnumerable<TourDto>> GetToursAsync(string language = "en-US");
    Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string language = "en-US");
    Task<TourDto?> GetTourByIdAsync(int id, string language = "en-US");
    Task<TourDto?> GetTourBySlugAsync(string slug, string language = "en-US");
    Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string language = "en-US");
    Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string language = "en-US");

    Task<string> TestBenchmarkAsync();
}