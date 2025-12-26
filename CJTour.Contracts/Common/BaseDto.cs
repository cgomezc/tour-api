using System.ComponentModel.DataAnnotations;

namespace CJTour.Contracts.Common;

public abstract class BaseDto<T>
{
    public T Id { get; set; } = default!;
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}