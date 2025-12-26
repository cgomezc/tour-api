using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class TourPackageDto : BaseDto<int>
{
    public int TourId { get; set; }
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Related data
    public ICollection<TourPackageScheduleDto> Schedules { get; set; } = new List<TourPackageScheduleDto>();
    public ICollection<TourPricingDto> Pricing { get; set; } = new List<TourPricingDto>();
}