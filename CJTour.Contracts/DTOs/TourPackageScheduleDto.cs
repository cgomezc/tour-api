using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class TourPackageScheduleDto : BaseDto<int>
{
    public int PackageId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int AvailableSeats { get; set; }
    public bool IsActive { get; set; }
}