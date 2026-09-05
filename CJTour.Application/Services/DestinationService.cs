using CJTour.Application.Interfaces;
using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;

namespace CJTour.Application.Services;

public class DestinationService : IDestinationService
{
    private readonly IDestinationRepository _destinationRepository;

    public DestinationService(IDestinationRepository destinationRepository)
    {
        _destinationRepository = destinationRepository;
    }

    public async Task<IEnumerable<DestinationDto>> GetAllDestinationsAsync(string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        var destinations = await _destinationRepository.GetAllAsync(languageCode);
        return destinations.Select(d => MapToDto(d, languageCode));
    }

    public async Task<DestinationDto?> GetDestinationByIdAsync(int destinationId, string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        var destination = await _destinationRepository.GetByIdAsync(destinationId);
        return destination != null ? MapToDto(destination, languageCode) : null;
    }

    public async Task<DestinationDto?> GetDestinationBySlugAsync(string slug, string languageCode = AppConstants.Culture.DefaultLanguage)
    {
        var destination = await _destinationRepository.GetBySlugAsync(slug);
        return destination != null ? MapToDto(destination, languageCode) : null;
    }

    private static DestinationDto MapToDto(Destination destination, string languageCode)
    {
        var translation = destination.DestinationTranslations
            .FirstOrDefault(t => t.LanguageCode == languageCode);

        return new DestinationDto
        {
            Id = destination.DestinationId,
            Name = translation?.Name ?? string.Empty,
            Description = translation?.Description,
            Country = destination.Country,
            City = destination.Region,
            Slug = destination.Slug,
            Latitude = destination.Latitude,
            Longitude = destination.Longitude,
            FeaturedImageUrl = destination.FeaturedImageUrl,
            Featured = destination.Featured ?? false,
            IsActive = destination.IsActive ?? false,
            CreatedAt = destination.CreatedAt,
            UpdatedAt = destination.UpdatedAt
        };
    }
}
