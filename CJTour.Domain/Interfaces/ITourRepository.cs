using CJTour.Domain.Entities;

namespace CJTour.Domain.Interfaces;

public interface ITourRepository
{
    Task<IEnumerable<Tour>> GetAllAsync(string languageCode = "en-US");
    Task<Tour?> GetByIdAsync(int tourId);
    Task<Tour?> GetBySlugAsync(string slug);
    Task<Tour?> GetByIdWithDetailsAsync(int tourId);
    Task<IEnumerable<Tour>> GetFeaturedToursAsync(string languageCode = "en-US");
    Task<IEnumerable<Tour>> GetToursByCategoryAsync(int categoryId, string languageCode = "en-US");
    Task<IEnumerable<Tour>> GetToursByDestinationAsync(int destinationId, string languageCode = "en-US");
    Task<Tour> AddAsync(Tour tour);
    /// <summary>
    /// Loads a tracked tour with only the data needed for an update (translations).
    /// </summary>
    Task<Tour?> GetForUpdateAsync(int tourId);
    /// <summary>
    /// Persists changes of entities already tracked by the context (e.g. loaded via <see cref="GetForUpdateAsync"/>).
    /// </summary>
    Task SaveChangesAsync();
    Task DeleteAsync(int tourId);
    Task<bool> ExistsAsync(int tourId);
    Task<bool> ExistsBySlugAsync(string slug);
    Task<bool> ExistsBySlugAsync(string slug, int excludeTourId);
}