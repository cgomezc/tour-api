namespace CJTour.Contracts.DTOs;

/// <summary>
/// Request payload for creating a new Tour and its translations.
/// Mirrors <see cref="UpdateTourDto"/> but additionally requires <see cref="DestinationId"/>,
/// since it is a NOT NULL foreign key on the "tours" table with no default value.
/// "CreatedBy" is intentionally not part of this payload: it is derived from the
/// authenticated user's claims in the endpoint, not supplied by the client.
/// </summary>
public class CreateTourDto
{
    /// <summary>
    /// Required. FK to destinations; NOT NULL in the database with no default value.
    /// </summary>
    public int DestinationId { get; set; }

    public string Slug { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal DurationHours { get; set; }
    public int DurationDays { get; set; }
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
