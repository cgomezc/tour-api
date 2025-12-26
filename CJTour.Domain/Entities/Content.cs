using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Content
{
    public int ContentId { get; set; }

    public int ContentTypeId { get; set; }

    public string Slug { get; set; } = null!;

    public int Position { get; set; }

    public int? ParentId { get; set; }

    public bool IsActive { get; set; }

    public string? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public virtual ContentType ContentType { get; set; } = null!;

    public virtual Content? Parent { get; set; }

    public virtual ICollection<Content> InverseParent { get; set; } = new List<Content>();

    public virtual ICollection<ContentTranslation> ContentTranslations { get; set; } = new List<ContentTranslation>();
}
