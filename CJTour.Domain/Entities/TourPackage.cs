using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourPackage
{
    public int PackageId { get; set; }

    public int TourId { get; set; }

    public string Code { get; set; } = null!;

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public virtual Tour Tour { get; set; } = null!;

    public virtual ICollection<TourPackageSchedule> TourPackageSchedules { get; set; } = new List<TourPackageSchedule>();

    public virtual ICollection<TourPackageTranslation> TourPackageTranslations { get; set; } = new List<TourPackageTranslation>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<TourPricing> TourPricings { get; set; } = new List<TourPricing>();
}
