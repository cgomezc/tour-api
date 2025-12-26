using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Participant
{
    public int ParticipantId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? PassportNumber { get; set; }

    public string? Nationality { get; set; }

    public string? DietaryRestrictions { get; set; }

    public string? SpecialRequirements { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<BookingParticipant> BookingParticipants { get; set; } = new List<BookingParticipant>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
