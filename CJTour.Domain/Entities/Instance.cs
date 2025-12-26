using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

/// <summary>
/// Auth: Manages users across multiple sites.
/// </summary>
public partial class Instance
{
    public int InstanceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
