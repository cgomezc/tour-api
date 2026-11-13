using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;

namespace CJTour.Application.Interfaces;

public interface ITourService
{
    Task<IEnumerable<TourDto>> GetAllToursAsync(string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None);
    Task<TourDto?> GetTourByIdAsync(int tourId, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None);
    Task<TourDto?> GetTourBySlugAsync(string slug, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None);
    Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None);
    Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None);
    Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None);
    Task<TourDto> CreateTourAsync(CreateTourDto createDto, string createdBy, string languageCode = AppConstants.Culture.DefaultLanguage);
    Task<TourDto?> UpdateTourAsync(int tourId, UpdateTourDto updateDto, string languageCode = AppConstants.Culture.DefaultLanguage);
    Task DeleteTourAsync(int tourId);
    Task<bool> TourExistsAsync(int tourId);
    Task<bool> TourExistsBySlugAsync(string slug);
}
