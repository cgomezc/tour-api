using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public string CartUuid { get; set; } = null!;

    public string? Currency { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
