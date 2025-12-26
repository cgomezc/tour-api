using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Language
{
    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<LanguageOffered> LanguageOffereds { get; set; } = new List<LanguageOffered>();
}
