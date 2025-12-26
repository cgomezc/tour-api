using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourTranslation
{
    public int Id { get; set; }

    public int TourId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Subtitle { get; set; }

    public string Description { get; set; } = null!;

    public string? DetailedItinerary { get; set; }

    public List<string>? Tags { get; set; }

    public string? MeetingPoint { get; set; }

    public List<string>? SuitableFor { get; set; }

    public string? ActivityLevel { get; set; }

    public List<string>? RequiredDocuments { get; set; }

    public List<string>? AccessibilityOptions { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }
   

    public virtual Tour Tour { get; set; } = null!;
}
