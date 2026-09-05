using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class TourCategory
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ParentCategoryId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<TourCategory> InverseParentCategory { get; set; } = new List<TourCategory>();

    public virtual TourCategory? ParentCategory { get; set; }

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
