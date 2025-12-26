using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Migration
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
