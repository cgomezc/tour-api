using System;

namespace CJTour.Domain.Entities;

public partial class TourPackageSchedule
{
    public int ScheduleId { get; set; }

    public int PackageId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int AvailableSeats { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TourPackage Package { get; set; } = null!;
}
