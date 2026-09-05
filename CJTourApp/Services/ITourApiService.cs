using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;

namespace CJTourApp.Services;

public interface ITourApiService
{
    Task<IEnumerable<TourDto>> GetToursAsync(string language = AppConstants.Culture.DefaultLanguage);
    Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string language = AppConstants.Culture.DefaultLanguage);
    Task<TourDto?> GetTourByIdAsync(int id, string language = AppConstants.Culture.DefaultLanguage);
    Task<TourDto?> GetTourBySlugAsync(string slug, string language = AppConstants.Culture.DefaultLanguage);
    Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string language = AppConstants.Culture.DefaultLanguage);
    Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string language = AppConstants.Culture.DefaultLanguage);

    Task<string> TestBenchmarkAsync();
}