using System.ComponentModel.DataAnnotations;
using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class DestinationDto : BaseDto<int>
{
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Country is required")]
    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
    [Display(Name = "Country")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
}