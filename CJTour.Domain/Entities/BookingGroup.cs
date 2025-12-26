using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class BookingGroup
{
    public int GroupId { get; set; }

    public int UserId { get; set; }

    public string GroupName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
