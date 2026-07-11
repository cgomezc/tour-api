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

    public async Task<IEnumerable<TourDto>> GetAllToursAsync(string languageCode = "en-US")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(languageCode, nameof(languageCode));

        try
        {
            var tours = await _tourRepository.GetAllAsync(languageCode);
            return tours.Select(tour => MapToDto(tour, languageCode));
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

    public async Task<TourDto?> GetTourByIdAsync(int tourId, string languageCode = "en-US")
    {
        var tour = await _tourRepository.GetByIdWithDetailsAsync(tourId, languageCode);
        return tour != null ? MapToDto(tour, languageCode) : null;
    }

    public async Task<TourDto?> GetTourBySlugAsync(string slug, string languageCode = "en-US")
    {
        var tour = await _tourRepository.GetBySlugAsync(slug);
        return tour != null ? MapToDto(tour, languageCode) : null;
    }

    public async Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string languageCode = "en-US")
    {
        var tours = await _tourRepository.GetFeaturedToursAsync(languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode));
    }

    public async Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string languageCode = "en-US")
    {
        var tours = await _tourRepository.GetToursByCategoryAsync(categoryId, languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode));
    }

    public async Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string languageCode = "en-US")
    {
        var tours = await _tourRepository.GetToursByDestinationAsync(destinationId, languageCode);
        return tours.Select(tour => MapToDto(tour, languageCode));
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

    private static TourDto MapToDto(Tour tour, string languageCode)
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
            Tags = translation?.Tags?.ToArray() ?? Array.Empty<string>(),
            MeetingPoint = translation?.MeetingPoint,
            SuitableFor = translation?.SuitableFor?.ToArray() ?? Array.Empty<string>(),
            ActivityLevel = translation?.ActivityLevel,
            RequiredDocuments = translation?.RequiredDocuments?.ToArray() ?? Array.Empty<string>(),
            AccessibilityOptions = translation?.AccessibilityOptions?.ToArray() ?? Array.Empty<string>(),
            SeoTitle = translation?.SeoTitle,
            SeoDescription = translation?.SeoDescription,
            
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
            
           Highlights = tour.TourHighlights.Select(h => new TourHighlightDto
           {
            Id = h.HighlightId,
            Name = h.TourHighlightTranslations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Title ?? string.Empty,
            CreatedAt = h.CreatedAt,
            UpdatedAt = h.UpdatedAt,
           }).ToList(),
                
            Packages = tour.TourPackages
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
                }).ToList(),
                
            LanguagesOffered = tour.LanguageOffereds
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
                }).ToList(),
                
            Pricing = tour.TourPricings
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
                }).ToList()
        };
    }
}