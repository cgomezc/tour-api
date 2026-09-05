
using CJTour.Application.Interfaces;
using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;

namespace CJTour.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ITourCategoryRepository _tourCategoryRepository;

        public CategoryService(ITourCategoryRepository tourCategoryRepository)
        {
            _tourCategoryRepository = tourCategoryRepository;
        }

        public async Task<IEnumerable<TourCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _tourCategoryRepository.GetAllAsync();
            return categories.Select(MapToDto);
        }

        private static TourCategoryDto MapToDto(TourCategory category)
        {
            return new TourCategoryDto
            {
                Id = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                ParentCategory = category.ParentCategory != null ? MapToDto(category.ParentCategory) : null,
                ChildCategories = category.InverseParentCategory.Select(MapToDto).ToList()
            };
        }
    }
}
