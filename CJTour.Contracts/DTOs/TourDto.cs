using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class TourDto : BaseDto<int>
{
    public string Slug { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal DurationHours { get; set; }
    public int DurationDays { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int[] AvailableDays { get; set; } = Array.Empty<int>();
    public int MinParticipants { get; set; }
    public int MaxParticipants { get; set; }
    public int? CurrentAvailability { get; set; }
    public int? MinAge { get; set; }
    public decimal? AverageRating { get; set; }
    public int? ReviewCount { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsFeatured { get; set; }
    public string TourCode { get; set; } = string.Empty;
    public string? RouteMapUrl { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public string[] GalleryImageUrls { get; set; } = Array.Empty<string>();
    public string? VideoUrl { get; set; }
    public string? TripAdvisorUlrReviews { get; set; }
    public bool? AcceptsMinors { get; set; }
    public bool Accessible { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? MeetingPoint { get; set; }
    public string[] SuitableFor { get; set; } = Array.Empty<string>();
    public string? ActivityLevel { get; set; }
    public string[] RequiredDocuments { get; set; } = Array.Empty<string>();
    public string[] AccessibilityOptions { get; set; } = Array.Empty<string>();
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }

    // Related data
    public DestinationDto? Destination { get; set; }
    public TourCategoryDto? Category { get; set; }
    public ICollection<TourTranslationDto> Translations { get; set; } = new List<TourTranslationDto>();
    public ICollection<TourHighlightDto> Highlights { get; set; } = new List<TourHighlightDto>();
    public ICollection<TourPackageDto> Packages { get; set; } = new List<TourPackageDto>();
    public ICollection<LanguageOfferedDto> LanguagesOffered { get; set; } = new List<LanguageOfferedDto>();
    public ICollection<TourPricingDto> Pricing { get; set; } = new List<TourPricingDto>();
}