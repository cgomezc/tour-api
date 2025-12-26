using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class AdditionalInfoTranslation
{
    public int TourAdditionalInfoTranslationId { get; set; }

    public int AdditionalInfoId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual AdditionalInfo AdditionalInfo { get; set; } = null!;
}
