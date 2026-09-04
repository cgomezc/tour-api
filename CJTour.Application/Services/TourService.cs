using CJTour.Application.Interfaces;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;
using CJTour.Contracts.DTOs;

namespace CJTour.Application.Services;

public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly ITourPackageRepository _tourPackageRepository;

    public TourService(ITourRepository tourRepository, ITourPackageRepository tourPackageRepository)
    {
        _tourRepository = tourRepository;
        _tourPackageRepository = tourPackageRepository;
    }

    public async Task<IEnumerable<TourDto>> GetAllToursAsync(string languageCode = "en-US", TourIncludeOptions include = TourIncludeOptions.None)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(languageCode, nameof(languageCode));

        try
        {
            var tours = await _tourRepository.GetAllAsync(languageCode);
            return tours.Select(tour => MapToDto(tour, languageCode, include));
        }
        catch (Exception ex) when (ex is not ArgumentException)
        {
            throw new InvalidOperationException(
                $"An error occurred while retrieving tours for language '{languageCode}'.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"An unexpected error occurred while retrieving tours for language '{languageCode}'.", ex);
        }
    }

    public async Task<TourDto?> GetTourByIdAsync(int tourId, string languageCode = "en-US", TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tour = await _tourRepository.GetByIdWithDetailsAsync(tourId);
        return tour != null ? MapToDto(tour, languageCode, include) : null;
    }

    public async Task<TourDto?> GetTourBySlugAsync(string slug, string languageCode = "en-US", TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tour = await _tourRepository.GetBySlugAsync(slug);
        return tour != null ? MapToDto(tour, languageCode, include) : null;
    }

    public async Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string languageCode = "en-US", TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tours = await _tourRepository.GetFeaturedToursAsync(languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode, include));
    }

    public async Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string languageCode = "en-US", TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tours = await _tourRepository.GetToursByCategoryAsync(categoryId, languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode, include));
    }

    public async Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string languageCode = "en-US", TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tours = await _tourRepository.GetToursByDestinationAsync(destinationId, languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode, include));
    }

    public async Task<TourDto?> UpdateTourAsync(int tourId, UpdateTourDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(updateDto, nameof(updateDto));

        var tour = await _tourRepository.GetByIdWithDetailsAsync(tourId);
        if (tour == null)
            return null;

        // Update Tour scalar fields
        tour.Slug = updateDto.Slug;
        tour.DurationHours = updateDto.DurationHours;
        tour.DurationDays = updateDto.DurationDays;

        if (DateTime.TryParse(updateDto.StartTime, out var startTime))
            tour.StartTime = startTime;

        if (DateTime.TryParse(updateDto.EndTime, out var endTime))
            tour.EndTime = endTime;

        if (updateDto.AvailableDays != null)
            tour.AvailableDays = updateDto.AvailableDays.ToList();

        if (updateDto.MinParticipants.HasValue)
            tour.MinParticipants = updateDto.MinParticipants.Value;

        if (updateDto.MaxParticipants.HasValue)
            tour.MaxParticipants = updateDto.MaxParticipants.Value;

        tour.CurrentAvailability = updateDto.CurrentAvailability ?? tour.CurrentAvailability;
        tour.MinAge = updateDto.MinAge ?? tour.MinAge;
        tour.IsActive = updateDto.IsActive ?? tour.IsActive;
        tour.IsFeatured = updateDto.IsFeatured ?? tour.IsFeatured;
        tour.TourCode = updateDto.TourCode ?? tour.TourCode;
        tour.RouteMapUrl = updateDto.RouteMapUrl ?? tour.RouteMapUrl;
        tour.FeaturedImageUrl = updateDto.FeaturedImageUrl ?? tour.FeaturedImageUrl;

        if (updateDto.GalleryImageUrls != null)
            tour.GalleryImageUrls = updateDto.GalleryImageUrls.ToList();

        tour.UpdatedAt = DateTime.UtcNow;

        // Upsert TourTranslations by LanguageCode, keyed to this tour's TourId
        if (updateDto.Translations != null)
        {
            foreach (var translationDto in updateDto.Translations)
            {
                var existingTranslation = tour.TourTranslations
                    .FirstOrDefault(t => t.LanguageCode == translationDto.LanguageCode);

                if (existingTranslation != null)
                {
                    existingTranslation.Title = translationDto.Title;
                    existingTranslation.Subtitle = translationDto.Subtitle;
                    existingTranslation.Description = translationDto.Description;
                    existingTranslation.DetailedItinerary = translationDto.DetailedItinerary;
                }
                else
                {
                    tour.TourTranslations.Add(new TourTranslation
                    {
                        TourId = tour.TourId,
                        LanguageCode = translationDto.LanguageCode,
                        Title = translationDto.Title,
                        Subtitle = translationDto.Subtitle,
                        Description = translationDto.Description,
                        DetailedItinerary = translationDto.DetailedItinerary
                    });
                }
            }
        }

        var updatedTour = await _tourRepository.UpdateAsync(tour);
        return MapToDto(updatedTour, "",TourIncludeOptions.Translations);
    }

    public async Task DeleteTourAsync(int tourId)
    {
        await _tourRepository.DeleteAsync(tourId);
    }

    public async Task<bool> TourExistsAsync(int tourId)
    {
        return await _tourRepository.ExistsAsync(tourId);
    }

    public async Task<bool> TourExistsBySlugAsync(string slug)
    {
        return await _tourRepository.ExistsBySlugAsync(slug);
    }

    private static TourDto MapToDto(Tour tour, string languageCode = "en-US", TourIncludeOptions include = TourIncludeOptions.None)
    {
        var translation = tour.TourTranslations.FirstOrDefault(t => t.LanguageCode == languageCode);

        return new TourDto
        {
            Id = tour.TourId,
            Name = translation?.Title ?? string.Empty,
            CreatedAt = tour.CreatedAt,
            UpdatedAt = tour.UpdatedAt,
            Slug = tour.Slug,
            Subtitle = translation?.Subtitle,
            Description = translation?.Description ?? string.Empty,
            DurationHours = tour.DurationHours,
            DurationDays = tour.DurationDays,
            StartTime = tour.StartTime,
            EndTime = tour.EndTime,
            AvailableDays = tour.AvailableDays?.ToArray() ?? [],
            MinParticipants = tour.MinParticipants,
            MaxParticipants = tour.MaxParticipants,
            CurrentAvailability = tour.CurrentAvailability,
            MinAge = tour.MinAge,
            AverageRating = tour.AverageRating,
            ReviewCount = tour.ReviewCount,
            IsActive = tour.IsActive,
            IsFeatured = tour.IsFeatured,
            TourCode = tour.TourCode,
            RouteMapUrl = tour.RouteMapUrl,
            FeaturedImageUrl = tour.FeaturedImageUrl,
            GalleryImageUrls = tour.GalleryImageUrls?.ToArray() ?? [],
            VideoUrl = tour.VideoUrl,
            TripAdvisorUlrReviews = tour.TripAdvisorUlrReviews,
            AcceptsMinors = tour.AcceptsMinors,
            Accessible = tour.Accessible,
            //Tags = translation?.Tags?.ToArray() ?? Array.Empty<string>(),
            //MeetingPoint = translation?.MeetingPoint,
            //SuitableFor = translation?.SuitableFor?.ToArray() ?? Array.Empty<string>(),
            //ActivityLevel = translation?.ActivityLevel,
            //RequiredDocuments = translation?.RequiredDocuments?.ToArray() ?? Array.Empty<string>(),
            //AccessibilityOptions = translation?.AccessibilityOptions?.ToArray() ?? Array.Empty<string>(),
            //SeoTitle = translation?.SeoTitle,
            //SeoDescription = translation?.SeoDescription,
            
            Destination = tour.Destination != null ? new DestinationDto
            {
                Id = tour.Destination.DestinationId,
                Name = tour.Destination.DestinationTranslations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Name ?? string.Empty,
                CreatedAt = tour.Destination.CreatedAt,
                UpdatedAt = tour.Destination.UpdatedAt,
                Description = tour.Destination.DestinationTranslations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Description ?? string.Empty,
                Country = tour.Destination.Country,
                City = tour.Destination.Region,
                IsActive = tour.Destination.IsActive ?? false
            } : null,
            
           Category = tour.Category != null ? new TourCategoryDto
           {
            Id = tour.Category.CategoryId,
            Name = tour.Category.Name,
            CreatedAt = tour.Category.CreatedAt,
            UpdatedAt = tour.Category.UpdatedAt,
           } : null,

            Translations = include.HasFlag(TourIncludeOptions.Translations)
                ? MapTranslations(tour)
                : new List<TourTranslationDto>(),

            Highlights = include.HasFlag(TourIncludeOptions.Highlights)
                ? MapHighlights(tour, languageCode)
                : new List<TourHighlightDto>(),

            Packages = include.HasFlag(TourIncludeOptions.Packages)
                ? MapPackages(tour, languageCode)
                : new List<TourPackageDto>(),

            LanguagesOffered = include.HasFlag(TourIncludeOptions.LanguagesOffered)
                ? MapLanguagesOffered(tour)
                : new List<LanguageOfferedDto>(),

            Pricing = include.HasFlag(TourIncludeOptions.Pricing)
                ? MapPricing(tour)
                : new List<TourPricingDto>()
        };
    }

    private static List<TourTranslationDto> MapTranslations(Tour tour)
    {
        return tour.TourTranslations.Select(t => new TourTranslationDto
        {
            Id = t.Id,
            TourId = t.TourId,
            LanguageCode = t.LanguageCode,
            Title = t.Title,
            Subtitle = t.Subtitle,
            Description = t.Description,
            DetailedItinerary = t.DetailedItinerary,
            Tags = t.Tags?.ToArray() ?? Array.Empty<string>(),
            MeetingPoint = t.MeetingPoint,
            SuitableFor = t.SuitableFor?.ToArray() ?? Array.Empty<string>(),
            ActivityLevel = t.ActivityLevel,
            RequiredDocuments = t.RequiredDocuments?.ToArray() ?? Array.Empty<string>(),
            AccessibilityOptions = t.AccessibilityOptions?.ToArray() ?? Array.Empty<string>(),
            SeoTitle = t.SeoTitle,
            SeoDescription = t.SeoDescription
        }).ToList();
    }

    private static List<TourHighlightDto> MapHighlights(Tour tour, string languageCode)
    {
        return tour.TourHighlights.Select(h => new TourHighlightDto
        {
            Id = h.HighlightId,
            Name = h.TourHighlightTranslations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Title ?? string.Empty,
            CreatedAt = h.CreatedAt,
            UpdatedAt = h.UpdatedAt,
        }).ToList();
    }

    private static List<TourPackageDto> MapPackages(Tour tour, string languageCode)
    {
        return tour.TourPackages
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .Select(p => new TourPackageDto
            {
                Id = p.PackageId,
                Name = p.TourPackageTranslations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Title ?? string.Empty,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                TourId = p.TourId,
                Code = p.Code,
                IsActive = p.IsActive,
                DisplayOrder = p.DisplayOrder,
                Title = p.TourPackageTranslations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Title ?? string.Empty,
                Description = p.TourPackageTranslations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Description ?? string.Empty,
            }).ToList();
    }

    private static List<LanguageOfferedDto> MapLanguagesOffered(Tour tour)
    {
        return tour.LanguageOffereds
            .Select(lo => new LanguageOfferedDto
            {
                Id = lo.LanguageId,
                Name = lo.Language?.Name ?? string.Empty,
                CreatedAt = lo.CreatedAt,
                UpdatedAt = lo.UpdatedAt,
                TourId = lo.TourId,
                LanguageId = lo.LanguageId,
                LanguageCode = lo.Language?.Code ?? string.Empty,
                LanguageName = lo.Language?.Name ?? string.Empty,
                Times = lo.Times?.ToArray() ?? [],
                IsPrimary = lo.IsPrimary,
                Notes = lo.Notes
            }).ToList();
    }

    private static List<TourPricingDto> MapPricing(Tour tour)
    {
        return tour.TourPricings
            .Where(p => p.TourId == tour.TourId)
            .Select(p => new TourPricingDto
            {
                Id = p.PricingId,
                Name = $"Pricing {p.PricingId}",
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                TourId = p.TourId,
                PackageId = p.PackageId,
                Price = p.PricePerAdult,
                Currency = p.Currency,
            }).ToList();
    }
}