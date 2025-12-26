using System.ComponentModel.DataAnnotations;
using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class InclusionsDto : BaseDto<int>
{
    [Required(ErrorMessage = "Category ID is required")]
    [Display(Name = "Category ID")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
    [Display(Name = "Code")]
    public string Code { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Icon URL cannot exceed 500 characters")]
    [Display(Name = "Icon")]
    public string? Icon { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Created By")]
    public string? CreatedBy { get; set; }

    [Display(Name = "Updated By")]
    public string? UpdatedBy { get; set; }
}

public class TourInclusionsDto : BaseDto<int>
{
    [Required(ErrorMessage = "Tour ID is required")]
    [Display(Name = "Tour ID")]
    public int TourId { get; set; }

    [Required(ErrorMessage = "Inclusion ID is required")]
    [Display(Name = "Inclusion ID")]
    public int InclusionId { get; set; }

    [Display(Name = "Is Included")]
    public bool IsIncluded { get; set; }

    [Display(Name = "Created By")]
    public string? CreatedBy { get; set; }
}

public class InclusionsCategoryDto : BaseDto<int>
{
    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }
}

public class InclusionTranslationDto : BaseDto<int>
{
    [Required(ErrorMessage = "Inclusion ID is required")]
    [Display(Name = "Inclusion ID")]
    public int InclusionId { get; set; }

    [Required(ErrorMessage = "Language code is required")]
    [StringLength(10, ErrorMessage = "Language code cannot exceed 10 characters")]
    [Display(Name = "Language Code")]
    public string LanguageCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    [Display(Name = "Name")]
    public string NameTranslation { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }
}