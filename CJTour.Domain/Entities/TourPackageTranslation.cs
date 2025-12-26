using System;

namespace CJTour.Domain.Entities;

public partial class TourPackageTranslation
{
    public int TourPackageTranslationId { get; set; }

    public int PackageId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TourPackage Package { get; set; } = null!;
}
