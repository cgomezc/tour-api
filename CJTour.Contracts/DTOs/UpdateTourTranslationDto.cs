namespace CJTour.Contracts.DTOs;

/// <summary>
/// Lightweight payload for updating a single TourTranslation as part of a Tour update.
/// Mirrors the fields the frontend actually sends/edits.
/// </summary>
public class UpdateTourTranslationDto
{
    public int? Id { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? DetailedItinerary { get; set; }
}
