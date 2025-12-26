using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourHighlight
{
    public int HighlightId { get; set; }

    public int TourId { get; set; }

    public int SortOrder { get; set; }

    public string? IconName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public virtual Tour Tour { get; set; } = null!;

    public virtual ICollection<TourHighlightTranslation> TourHighlightTranslations { get; set; } = new List<TourHighlightTranslation>();
}
