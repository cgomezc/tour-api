using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int TourId { get; set; }

    public int UserId { get; set; }

    public int? GroupId { get; set; }

    public int? PackageId { get; set; }

    public int? ParticipantId { get; set; }

    public int? PickupLocationId { get; set; }

    public int? PricingId { get; set; }

    public string Status { get; set; } = null!;

    public string BookingReference { get; set; } = null!;

    public DateTime BookingDate { get; set; }

    public int Adults { get; set; }

    public int Children { get; set; }

    public int Infants { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Tour Tour { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual BookingGroup? Group { get; set; }

    public virtual Participant? Participant { get; set; }

    public virtual PickupLocation? PickupLocation { get; set; }

    public virtual TourPricing? Pricing { get; set; }

    public virtual ICollection<BookingParticipant> BookingParticipants { get; set; } = new List<BookingParticipant>();
}
