using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourInclusion
{
    public int TourInclusionId { get; set; }

    public int TourId { get; set; }

    public int InclusionId { get; set; }

    public bool IsIncluded { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;

    public virtual Inclusion Inclusion { get; set; } = null!;
}
