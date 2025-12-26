using System;

namespace CJTour.Domain.Entities;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int TourId { get; set; }

    public int? PackageId { get; set; }

    public int Adults { get; set; }

    public int Children { get; set; }

    public int Infants { get; set; }

    public decimal PricePerAdult { get; set; }

    public decimal PricePerChild { get; set; }

    public decimal PricePerInfant { get; set; }

    public decimal ItemTotal { get; set; }

    public DateTime? BookingDate { get; set; }

    public string? PickupLocationName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;

    public virtual TourPackage? Package { get; set; }
}
