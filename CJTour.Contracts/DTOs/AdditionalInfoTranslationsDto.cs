using System.ComponentModel.DataAnnotations;
using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class AdditionalInfoTranslationsDto : BaseDto<int>
{
    [Required(ErrorMessage = "Additional Info ID is required")]
    [Display(Name = "Additional Info ID")]
    public int AdditionalInfoId { get; set; }

    [Required(ErrorMessage = "Language code is required")]
    [StringLength(10, ErrorMessage = "Language code cannot exceed 10 characters")]
    [Display(Name = "Language Code")]
    public string LanguageCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
}