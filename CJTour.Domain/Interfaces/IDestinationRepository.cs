using CJTour.Domain.Entities;

namespace CJTour.Domain.Interfaces;

public interface IDestinationRepository
{
    Task<IEnumerable<Destination>> GetAllAsync(string languageCode = "en-US");
    Task<Destination?> GetByIdAsync(int destinationId);
    Task<Destination?> GetBySlugAsync(string slug);
    Task<bool> ExistsAsync(int destinationId);
    Task<bool> ExistsBySlugAsync(string slug);
}
