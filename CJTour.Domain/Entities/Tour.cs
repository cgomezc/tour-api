using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Tour
{
    public List<int>? AvailableDays { get; set; }

    public decimal? AverageRating { get; set; }

    public int? CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public int? CurrentAvailability { get; set; }

    public int DestinationId { get; set; }

    public int DurationDays { get; set; }

    public decimal DurationHours { get; set; }

    public DateTime? EndTime { get; set; }

    public string? FeaturedImageUrl { get; set; }

    public List<string>? GalleryImageUrls { get; set; }

    public bool IsActive { get; set; }

    public bool? IsFeatured { get; set; }

    public int MaxParticipants { get; set; }

    public int? MinAge { get; set; }

    public int MinParticipants { get; set; }

    public int? ReviewCount { get; set; }

    public string? RouteMapUrl { get; set; }

    public string Slug { get; set; } = null!;

    public DateTime? StartTime { get; set; }

    public string? TourCode { get; set; }

    public int TourId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public string? VideoUrl { get; set; }

    public bool? AcceptsMinors { get; set; }

    public bool Accessible { get; set; }

    public int? LocationId { get; set; }

    public string? TripAdvisorUlrReviews { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Tourcategory? Category { get; set; }

    public virtual Destination Destination { get; set; } = null!;

    public virtual ICollection<LanguageOffered> LanguageOffereds { get; set; } = new List<LanguageOffered>();

    public virtual ICollection<TourAdditionalInfo> TourAdditionalInfos { get; set; } = new List<TourAdditionalInfo>();

    public virtual ICollection<TourHighlight> TourHighlights { get; set; } = new List<TourHighlight>();

    public virtual ICollection<TourInclusion> TourInclusions { get; set; } = new List<TourInclusion>();

    public virtual ICollection<TourPackage> TourPackages { get; set; } = new List<TourPackage>();

    public virtual ICollection<TourPricing> TourPricings { get; set; } = new List<TourPricing>();

    public virtual ICollection<TourTranslation> TourTranslations { get; set; } = new List<TourTranslation>();

    public virtual ICollection<Tourdateblock> Tourdateblocks { get; set; } = new List<Tourdateblock>();
}
