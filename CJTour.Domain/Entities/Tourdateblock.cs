using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Tourdateblock
{
    public int TourDateBlockId { get; set; }

    public int TourId { get; set; }

    public DateTime BlockedDate { get; set; }

    public string? Reason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;
}
