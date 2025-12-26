using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class InclusionsCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Inclusion> Inclusions { get; set; } = new List<Inclusion>();
}
