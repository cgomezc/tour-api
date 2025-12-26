using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Inclusion
{
    public int InclusionId { get; set; }

    public int CategoryId { get; set; }

    public string Code { get; set; } = null!;

    public string? Icon { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual InclusionsCategory Category { get; set; } = null!;

    public virtual ICollection<TourInclusion> TourInclusions { get; set; } = new List<TourInclusion>();

    public virtual ICollection<InclusionTranslation> InclusionTranslations { get; set; } = new List<InclusionTranslation>();
}
