namespace CJTour.Application.Interfaces;

/// <summary>
/// Controls which optional related collections are populated when mapping a Tour to a TourDto.
/// None of these collections are included by default; callers must opt in explicitly by
/// combining the desired flags (e.g. TourIncludeOptions.Highlights | TourIncludeOptions.Packages).
/// </summary>
[Flags]
public enum TourIncludeOptions
{
    None = 0,
    Translations = 1 << 0,
    Highlights = 1 << 1,
    Packages = 1 << 2,
    LanguagesOffered = 1 << 3,
    Pricing = 1 << 4,
    All = Translations | Highlights | Packages | LanguagesOffered | Pricing
}
