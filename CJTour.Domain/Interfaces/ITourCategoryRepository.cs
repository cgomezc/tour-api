using CJTour.Domain.Entities;

namespace CJTour.Domain.Interfaces;

public interface ITourCategoryRepository
{
    Task<IEnumerable<TourCategory>> GetAllAsync();
    Task<TourCategory?> GetByIdAsync(int categoryId);
    Task<TourCategory?> GetByIdWithChildrenAsync(int categoryId);
    Task<bool> ExistsAsync(int categoryId);
}
