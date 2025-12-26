using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Destination
{
    public int DestinationId { get; set; }

    public string Slug { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Region { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? FeaturedImageUrl { get; set; }

    public bool? IsActive { get; set; }

    public bool? Featured { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DestinationTranslation> DestinationTranslations { get; set; } = new List<DestinationTranslation>();

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
