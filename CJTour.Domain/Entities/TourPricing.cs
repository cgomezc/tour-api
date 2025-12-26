using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourPricing
{
    public int PricingId { get; set; }

    public int TourId { get; set; }

    public int? PackageId { get; set; }

    public decimal PricePerAdult { get; set; }

    public decimal? PricePerChild { get; set; }

    public decimal? PricePerInfant { get; set; }

    public string Currency { get; set; } = null!;

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool? IsCurrent { get; set; }

    public int? MaxParticipants { get; set; }

    public int? MinParticipants { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;

    public virtual TourPackage? Package { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
