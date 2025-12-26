using System.ComponentModel.DataAnnotations;
using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class AdditionalInfoDto : BaseDto<int>
{
    [Required(ErrorMessage = "Info type is required")]
    [StringLength(100, ErrorMessage = "Info type cannot exceed 100 characters")]
    [Display(Name = "Info Type")]
    public string InfoType { get; set; } = string.Empty;
}