using CJTour.Contracts.Common;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;
using CJTour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CJTour.Infrastructure.Persistence.Repositories;

public class DestinationRepository : IDestinationRepository
{
    private readonly AppDbContext _context;

    public DestinationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Destination>> GetAllAsync(string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        return await _context.Destinations
            .AsNoTracking()
            .Include(d => d.DestinationTranslations.Where(dt => dt.LanguageCode == languageCode))
            .Where(d => d.IsActive == true)
            .ToListAsync();
    }

    public async Task<Destination?> GetByIdAsync(int destinationId)
    {
        return await _context.Destinations
            .AsNoTracking()
            .Include(d => d.DestinationTranslations)
            .FirstOrDefaultAsync(d => d.DestinationId == destinationId && d.IsActive == true);
    }

    public async Task<Destination?> GetBySlugAsync(string slug)
    {
        return await _context.Destinations
            .AsNoTracking()
            .Include(d => d.DestinationTranslations)
            .FirstOrDefaultAsync(d => d.Slug == slug && d.IsActive == true);
    }

    public async Task<bool> ExistsAsync(int destinationId)
    {
        return await _context.Destinations
            .AnyAsync(d => d.DestinationId == destinationId && d.IsActive == true);
    }

    public async Task<bool> ExistsBySlugAsync(string slug)
    {
        return await _context.Destinations
            .AnyAsync(d => d.Slug == slug && d.IsActive == true);
    }
}
