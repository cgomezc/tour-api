using CJTour.Contracts.Common;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;
using CJTour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace CJTour.Infrastructure.Persistence.Repositories;

public class TourRepository : ITourRepository
{
    private readonly AppDbContext _context;

    public TourRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tour>> GetAllAsync(string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        try
        {
            return await _context.Tours
                 .Include(t => t.TourTranslations.Where(tt => tt.LanguageCode == languageCode))
                .Where(t => t.IsActive == true)
                .ToListAsync();
        }
        catch (SqlTypeException se)
        {

            Console.WriteLine(se);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task<Tour?> GetByIdAsync(int tourId)
    {
        return await _context.Tours
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .Include(t => t.TourTranslations)
            .Include(t => t.TourHighlights)
            .Include(t => t.TourPackages)
            .Include(t => t.LanguageOffereds)
            .Include(t => t.TourPricings)
            .FirstOrDefaultAsync(t => t.TourId == tourId && t.IsActive == true);
    }

    public async Task<Tour?> GetBySlugAsync(string slug)
    {
        return await _context.Tours
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .Include(t => t.TourTranslations)
            .Include(t => t.TourHighlights)
            .Include(t => t.TourPackages)
            .Include(t => t.LanguageOffereds)
            .Include(t => t.TourPricings)
            .FirstOrDefaultAsync(t => t.Slug == slug && t.IsActive == true);
    }

    public async Task<Tour?> GetByIdWithDetailsAsync(int tourId)
    {
        return await _context.Tours
            .Include(t => t.Destination)
                .ThenInclude(d => d.DestinationTranslations)
            .Include(t => t.Category)
            .Include(t => t.TourTranslations)
            .Include(t => t.TourHighlights)
                .ThenInclude(th => th.TourHighlightTranslations)
            .Include(t => t.TourPackages.Where(tp => tp.IsActive))
                .ThenInclude(tp => tp.TourPackageTranslations)
            .Include(t => t.LanguageOffereds)
                .ThenInclude(lo => lo.Language)
            .Include(t => t.TourPricings.Where(tpr => tpr.IsCurrent == true))
            .FirstOrDefaultAsync(t => t.TourId == tourId && t.IsActive == true);
    }

    public async Task<IEnumerable<Tour>> GetFeaturedToursAsync(string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        return await _context.Tours
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .Include(t => t.TourTranslations.Where(tt => tt.LanguageCode == languageCode))
            .Include(t => t.TourHighlights)
            .Include(t => t.TourPackages)
            .Include(t => t.LanguageOffereds)
            .Include(t => t.TourPricings)
            .Where(t => t.IsActive == true && t.IsFeatured == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tour>> GetToursByCategoryAsync(int categoryId, string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        return await _context.Tours
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .Include(t => t.TourTranslations.Where(tt => tt.LanguageCode == languageCode))
            .Include(t => t.TourHighlights)
            .Include(t => t.TourPackages)
            .Include(t => t.LanguageOffereds)
            .Include(t => t.TourPricings)
            .Where(t => t.CategoryId == categoryId && t.IsActive == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tour>> GetToursByDestinationAsync(int destinationId, string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        return await _context.Tours
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .Include(t => t.TourTranslations.Where(tt => tt.LanguageCode == languageCode))
            .Include(t => t.TourHighlights)
            .Include(t => t.TourPackages)
            .Include(t => t.LanguageOffereds)
            .Include(t => t.TourPricings)
            .Where(t => t.DestinationId == destinationId && t.IsActive == true)
            .ToListAsync();
    }

    public async Task<Tour> AddAsync(Tour tour)
    {
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();
        return tour;
    }

    public async Task<Tour> UpdateAsync(Tour tour)
    {
        _context.Tours.Update(tour);
        await _context.SaveChangesAsync();
        return tour;
    }

    public async Task DeleteAsync(int tourId)
    {
        var tour = await _context.Tours.FindAsync(tourId);
        if (tour != null)
        {
            tour.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int tourId)
    {
        return await _context.Tours.AnyAsync(t => t.TourId == tourId && t.IsActive == true);
    }

    public async Task<bool> ExistsBySlugAsync(string slug)
    {
        return await _context.Tours.AnyAsync(t => t.Slug == slug && t.IsActive == true);
    }
}