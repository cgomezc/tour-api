using System.ComponentModel.DataAnnotations;
using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class PickupLocationDto : BaseDto<int>
{
    [Required(ErrorMessage = "Location name is required")]
    [StringLength(200, ErrorMessage = "Location name cannot exceed 200 characters")]
    [Display(Name = "Location Name")]
    public string LocationName { get; set; } = string.Empty;

    [Display(Name = "Pickup Time")]
    public DateTime? PickupTime { get; set; }

    [StringLength(500, ErrorMessage = "Instructions cannot exceed 500 characters")]
    [Display(Name = "Instructions")]
    public string? Instructions { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }
}