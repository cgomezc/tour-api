using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class BookingParticipant
{
    public int BookingParticipantId { get; set; }

    public int BookingId { get; set; }

    public int ParticipantId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Participant Participant { get; set; } = null!;
}
