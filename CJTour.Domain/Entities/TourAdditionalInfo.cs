using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourAdditionalInfo
{
    public int TourAdditionalInfoId { get; set; }

    public int TourId { get; set; }

    public int AdditionalInfoId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual AdditionalInfo AdditionalInfo { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;
}
