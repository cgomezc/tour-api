using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class ContentTranslation
{
    public int ContentTranslationId { get; set; }

    public int ContentId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Subtitle { get; set; }

    public string Content { get; set; } = null!;

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Content ContentNavigation { get; set; } = null!;
}
