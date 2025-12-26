using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class LanguageOffered
{
    public int LanguageOfferedId { get; set; }

    public int TourId { get; set; }

    public int LanguageId { get; set; }

    public bool IsActive { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsPrimary { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<string>? Times { get; set; }

    public virtual Tour Tour { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
