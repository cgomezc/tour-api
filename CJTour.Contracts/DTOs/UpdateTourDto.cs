namespace CJTour.Contracts.DTOs;

/// <summary>
/// Lightweight request payload for updating a Tour and its translations.
/// Only includes fields the frontend actually edits; unlike <see cref="TourDto"/>
/// it omits read-only/computed fields (e.g. AverageRating, ReviewCount) and
/// related collections (Destination, Category, Highlights, Packages, Pricing, etc.).
/// The tour is identified by the route "tourId", not by a field in this payload.
/// </summary>
public class UpdateTourDto
{
    public int? DestinationId { get; set; }
    public string? Slug { get; set; }
    public string? Subtitle { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? DurationHours { get; set; }
    public int? DurationDays { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public int[]? AvailableDays { get; set; }
    public int? MinParticipants { get; set; }
    public int? MaxParticipants { get; set; }
    public int? CurrentAvailability { get; set; }
    public int? MinAge { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsFeatured { get; set; }
    public string? TourCode { get; set; }
    public string? RouteMapUrl { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public string[]? GalleryImageUrls { get; set; }

    public List<UpdateTourTranslationDto>? Translations { get; set; }
}
