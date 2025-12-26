using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class TourPricingDto : BaseDto<int>
{
    public int TourId { get; set; }
    public int? PackageId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? PricingType { get; set; }
    public bool IsActive { get; set; }
}