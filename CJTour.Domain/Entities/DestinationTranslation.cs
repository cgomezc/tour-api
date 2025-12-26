using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class DestinationTranslation
{
    public int Id { get; set; }

    public int DestinationId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Destination Destination { get; set; } = null!;
}
