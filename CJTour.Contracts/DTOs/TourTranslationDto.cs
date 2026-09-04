namespace CJTour.Contracts.DTOs;

public class TourTranslationDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? DetailedItinerary { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string? MeetingPoint { get; set; }
    public string[] SuitableFor { get; set; } = Array.Empty<string>();
    public string? ActivityLevel { get; set; }
    public string[] RequiredDocuments { get; set; } = Array.Empty<string>();
    public string[] AccessibilityOptions { get; set; } = Array.Empty<string>();
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
}
