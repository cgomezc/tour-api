using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class TourCategoryDto : BaseDto<int>
{
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    
    // Navigation properties
    public TourCategoryDto? ParentCategory { get; set; }
    public ICollection<TourCategoryDto> ChildCategories { get; set; } = new List<TourCategoryDto>();
}