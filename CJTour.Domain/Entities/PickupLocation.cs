using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class PickupLocation
{
    public int PickUpLocationId { get; set; }

    public string LocationName { get; set; } = null!;

    public DateTime? PickupTime { get; set; }

    public string? Instructions { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
