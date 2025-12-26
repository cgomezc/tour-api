using System.ComponentModel.DataAnnotations;
using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class TourAdditionalInfoDto : BaseDto<int>
{
    [Required(ErrorMessage = "Tour ID is required")]
    [Display(Name = "Tour ID")]
    public int TourId { get; set; }

    [Required(ErrorMessage = "Additional Info ID is required")]
    [Display(Name = "Additional Info ID")]
    public int AdditionalInfoId { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }
}