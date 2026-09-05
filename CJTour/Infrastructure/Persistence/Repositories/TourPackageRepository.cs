using CJTour.Contracts.Common;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;
using CJTour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CJTour.Infrastructure.Persistence.Repositories;

public class TourPackageRepository : ITourPackageRepository
{
    private readonly AppDbContext _context;

    public TourPackageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TourPackage>> GetByTourIdAsync(int tourId)
    {
        return await _context.TourPackages
            .Include(tp => tp.TourPackageTranslations)
            .Include(tp => tp.TourPackageSchedules)
            .Include(tp => tp.TourPricings)
            .Where(tp => tp.TourId == tourId && tp.IsActive)
            .OrderBy(tp => tp.DisplayOrder)
            .ToListAsync();
    }

    public async Task<TourPackage?> GetByIdAsync(int packageId)
    {
        return await _context.TourPackages
            .Include(tp => tp.TourPackageTranslations)
            .Include(tp => tp.TourPackageSchedules)
            .Include(tp => tp.TourPricings)
            .FirstOrDefaultAsync(tp => tp.PackageId == packageId && tp.IsActive);
    }

    public async Task<TourPackage?> GetByIdWithDetailsAsync(int packageId, string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        return await _context.TourPackages
            .Include(tp => tp.Tour)
            .Include(tp => tp.TourPackageTranslations.Where(tpt => tpt.LanguageCode == languageCode))
            .Include(tp => tp.TourPackageSchedules.Where(tps => tps.IsActive))
            .Include(tp => tp.TourPricings.Where(tpr => tpr.IsCurrent == true))
            .FirstOrDefaultAsync(tp => tp.PackageId == packageId && tp.IsActive);
    }

    public async Task<TourPackage> AddAsync(TourPackage tourPackage)
    {
        _context.TourPackages.Add(tourPackage);
        await _context.SaveChangesAsync();
        return tourPackage;
    }

    public async Task<TourPackage> UpdateAsync(TourPackage tourPackage)
    {
        _context.TourPackages.Update(tourPackage);
        await _context.SaveChangesAsync();
        return tourPackage;
    }

    public async Task DeleteAsync(int packageId)
    {
        var package = await _context.TourPackages.FindAsync(packageId);
        if (package != null)
        {
            package.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int packageId)
    {
        return await _context.TourPackages.AnyAsync(tp => tp.PackageId == packageId && tp.IsActive);
    }
} 