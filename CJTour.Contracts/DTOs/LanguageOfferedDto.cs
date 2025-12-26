using CJTour.Contracts.Common;

namespace CJTour.Contracts.DTOs;

public class LanguageOfferedDto : BaseDto<int>
{
    public int TourId { get; set; }
    public int LanguageId { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public string LanguageName { get; set; } = string.Empty;
    public string[] Times { get; set; } = Array.Empty<string>();
    public bool IsPrimary { get; set; }
    public string? Notes { get; set; }
}