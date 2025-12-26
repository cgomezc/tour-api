using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourHighlightTranslation
{
    public int Id { get; set; }

    public int HighlightId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TourHighlight Highlight { get; set; } = null!;
}
