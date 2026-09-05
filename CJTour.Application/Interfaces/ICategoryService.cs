
using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;

namespace CJTour.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<TourCategoryDto>> GetAllCategoriesAsync();
    }
}
