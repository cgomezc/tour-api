using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;
using CJTour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CJTour.Infrastructure.Persistence.Repositories;

public class TourCategoryRepository : ITourCategoryRepository
{
    private readonly AppDbContext _context;

    public TourCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TourCategory>> GetAllAsync()
    {
        return await _context.Tourcategories
            .AsNoTracking()           
            .ToListAsync();
    }

    public async Task<TourCategory?> GetByIdAsync(int categoryId)
    {
        return await _context.Tourcategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }

    public async Task<TourCategory?> GetByIdWithChildrenAsync(int categoryId)
    {
        return await _context.Tourcategories
            .AsNoTracking()
            .Include(c => c.ParentCategory)
            .Include(c => c.InverseParentCategory)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }

    public async Task<bool> ExistsAsync(int categoryId)
    {
        return await _context.Tourcategories
            .AnyAsync(c => c.CategoryId == categoryId);
    }
}
