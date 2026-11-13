using CJTour.Application.Interfaces;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;
using CJTour.Contracts.DTOs;
using CJTour.Contracts.Common;
using CJTour.Application.Mappings;

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

    public async Task<IEnumerable<TourDto>> GetAllToursAsync(string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None)
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

    public async Task<TourDto?> GetTourByIdAsync(int tourId, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tour = await _tourRepository.GetByIdWithDetailsAsync(tourId);
        return tour != null ? MapToDto(tour, languageCode, include) : null;
    }

    public async Task<TourDto?> GetTourBySlugAsync(string slug, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tour = await _tourRepository.GetBySlugAsync(slug);
        return tour != null ? MapToDto(tour, languageCode, include) : null;
    }

    public async Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tours = await _tourRepository.GetFeaturedToursAsync(languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode, include));
    }

    public async Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tours = await _tourRepository.GetToursByCategoryAsync(categoryId, languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode, include));
    }

    public async Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None)
    {
        var tours = await _tourRepository.GetToursByDestinationAsync(destinationId, languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode, include));
    }

    public async Task<TourDto> CreateTourAsync(CreateTourDto createDto, string createdBy, string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        ArgumentNullException.ThrowIfNull(createDto, nameof(createDto));
        ArgumentException.ThrowIfNullOrWhiteSpace(createdBy, nameof(createdBy));

        if (createDto.DestinationId <= 0)
            throw new ArgumentException("DestinationId is required.", nameof(createDto));

        if (string.IsNullOrWhiteSpace(createDto.Slug))
            throw new ArgumentException("Slug is required.", nameof(createDto));

        if (await _tourRepository.ExistsBySlugAsync(createDto.Slug))
            throw new ArgumentException($"A tour with slug '{createDto.Slug}' already exists.", nameof(createDto));

        if (createDto.Translations == null || createDto.Translations.Count == 0)
            throw new ArgumentException("At least one translation is required.", nameof(createDto));

        var tour = new Tour
        {
            DestinationId = createDto.DestinationId,
            Slug = createDto.Slug,
            DurationHours = createDto.DurationHours,
            DurationDays = createDto.DurationDays,
            AvailableDays = createDto.AvailableDays?.ToList(),
            MinParticipants = createDto.MinParticipants ?? 1,
            MaxParticipants = createDto.MaxParticipants ?? 1,
            CurrentAvailability = createDto.CurrentAvailability,
            MinAge = createDto.MinAge,
            IsActive = createDto.IsActive ?? true,
            IsFeatured = createDto.IsFeatured,
            TourCode = createDto.TourCode,
            RouteMapUrl = createDto.RouteMapUrl,
            FeaturedImageUrl = createDto.FeaturedImageUrl,
            GalleryImageUrls = createDto.GalleryImageUrls?.ToList(),
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };

        if (TourUpdateMapper.TryParseDateTime(createDto.StartTime, out var startTime))
            tour.StartTime = startTime;

        if (TourUpdateMapper.TryParseDateTime(createDto.EndTime, out var endTime))
            tour.EndTime = endTime;

        foreach (var translationDto in createDto.Translations)
            tour.TourTranslations.Add(translationDto.ToEntity());

        var createdTour = await _tourRepository.AddAsync(tour);
        return MapToDto(createdTour, languageCode, TourIncludeOptions.Translations);
    }

    public async Task<TourDto?> UpdateTourAsync(int tourId, UpdateTourDto updateDto, string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        ArgumentNullException.ThrowIfNull(updateDto, nameof(updateDto));

        var tour = await _tourRepository.GetForUpdateAsync(tourId);
        if (tour == null)
            return null;

        if (updateDto.DestinationId is <= 0)
            throw new ArgumentException("DestinationId must be greater than zero.", nameof(updateDto));

        if (updateDto.Slug is not null)
        {
            if (string.IsNullOrWhiteSpace(updateDto.Slug))
                throw new ArgumentException("Slug cannot be empty.", nameof(updateDto));

            if (updateDto.Slug != tour.Slug && await _tourRepository.ExistsBySlugAsync(updateDto.Slug, tourId))
                throw new ArgumentException($"A tour with slug '{updateDto.Slug}' already exists.", nameof(updateDto));
        }

        updateDto.ApplyTo(tour);
        tour.UpdatedAt = DateTime.UtcNow;

        if (updateDto.Translations != null)
        {
            foreach (var translationDto in updateDto.Translations)
            {
                if (string.IsNullOrWhiteSpace(translationDto.LanguageCode))
                    throw new ArgumentException("Each translation requires a LanguageCode.", nameof(updateDto));

                var existing = tour.TourTranslations.FirstOrDefault(t =>
                    string.Equals(t.LanguageCode, translationDto.LanguageCode, StringComparison.OrdinalIgnoreCase));

                if (existing != null)
                    translationDto.ApplyTo(existing);
                else
                    tour.TourTranslations.Add(translationDto.ToEntity());
            }
        }

        await _tourRepository.SaveChangesAsync();

        // Reload with full details for the response; fall back to the tracked entity
        // if the tour is no longer returned (e.g. it was just deactivated).
        return await GetTourByIdAsync(tourId, languageCode, TourIncludeOptions.Translations)
               ?? MapToDto(tour, languageCode, TourIncludeOptions.Translations);
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

    private static TourDto MapToDto(Tour tour, string languageCode = AppConstants.Culture.DefaultLanguage, TourIncludeOptions include = TourIncludeOptions.None)
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