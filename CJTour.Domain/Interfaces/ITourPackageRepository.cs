using CJTour.Domain.Entities;

namespace CJTour.Domain.Interfaces;

public interface ITourPackageRepository
{
    Task<IEnumerable<TourPackage>> GetByTourIdAsync(int tourId);
    Task<TourPackage?> GetByIdAsync(int packageId);
    Task<TourPackage?> GetByIdWithDetailsAsync(int packageId, string languageCode = "en-US");
    Task<TourPackage> AddAsync(TourPackage tourPackage);
    Task<TourPackage> UpdateAsync(TourPackage tourPackage);
    Task DeleteAsync(int packageId);
    Task<bool> ExistsAsync(int packageId);
}