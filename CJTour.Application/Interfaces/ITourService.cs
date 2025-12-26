using CJTour.Contracts.DTOs;

namespace CJTour.Application.Interfaces;

public interface ITourService
{
    Task<IEnumerable<TourDto>> GetAllToursAsync(string languageCode = "en-US");
    Task<TourDto?> GetTourByIdAsync(int tourId, string languageCode = "en-US");
    Task<TourDto?> GetTourBySlugAsync(string slug, string languageCode = "en-US");
    Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string languageCode = "en-US");
    Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string languageCode = "en-US");
    Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string languageCode = "en-US");
    // Task<TourDto> CreateTourAsync(TourDto tourDto);
    // Task<TourDto> UpdateTourAsync(int tourId, TourDto tourDto);
    Task DeleteTourAsync(int tourId);
    Task<bool> TourExistsAsync(int tourId);
    Task<bool> TourExistsBySlugAsync(string slug);
}
