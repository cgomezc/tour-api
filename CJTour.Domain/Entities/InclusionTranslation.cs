using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class InclusionTranslation
{
    public int InclusionTranslationId { get; set; }

    public int InclusionId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Inclusion Inclusion { get; set; } = null!;
}
