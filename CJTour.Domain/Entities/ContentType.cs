using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class ContentType
{
    public int ContentTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();
}
