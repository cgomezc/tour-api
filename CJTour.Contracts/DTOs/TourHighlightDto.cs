using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class TourHighlightDto : BaseDto<int>
{
    public int TourId { get; set; }
    public int SortOrder { get; set; }
    public string? IconName { get; set; }
    public bool IsActive { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}