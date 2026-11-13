using System.Globalization;
using CJTour.Contracts.DTOs;
using CJTour.Domain.Entities;

namespace CJTour.Application.Mappings;

/// <summary>
/// Explicit, compile-time checked mapping for partial (PATCH-style) tour updates.
/// A <c>null</c> value in the DTO means "not sent / keep current value".
/// Keys, foreign keys of children and audit fields are never touched here.
/// </summary>
public static class TourUpdateMapper
{
    public static void ApplyTo(this UpdateTourDto dto, Tour tour)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(tour);

        if (dto.DestinationId is { } destinationId) tour.DestinationId = destinationId;
        if (dto.Slug is { } slug) tour.Slug = slug;
        if (dto.DurationHours is { } durationHours) tour.DurationHours = durationHours;
        if (dto.DurationDays is { } durationDays) tour.DurationDays = durationDays;
        if (dto.AvailableDays is { } availableDays) tour.AvailableDays = availableDays.ToList();
        if (dto.MinParticipants is { } minParticipants) tour.MinParticipants = minParticipants;
        if (dto.MaxParticipants is { } maxParticipants) tour.MaxParticipants = maxParticipants;
        if (dto.CurrentAvailability is { } currentAvailability) tour.CurrentAvailability = currentAvailability;
        if (dto.MinAge is { } minAge) tour.MinAge = minAge;
        if (dto.IsActive is { } isActive) tour.IsActive = isActive;
        if (dto.IsFeatured is { } isFeatured) tour.IsFeatured = isFeatured;
        if (dto.TourCode is { } tourCode) tour.TourCode = tourCode;
        if (dto.RouteMapUrl is { } routeMapUrl) tour.RouteMapUrl = routeMapUrl;
        if (dto.FeaturedImageUrl is { } featuredImageUrl) tour.FeaturedImageUrl = featuredImageUrl;
        if (dto.GalleryImageUrls is { } galleryImageUrls) tour.GalleryImageUrls = galleryImageUrls.ToList();

        if (TryParseDateTime(dto.StartTime, out var startTime)) tour.StartTime = startTime;
        if (TryParseDateTime(dto.EndTime, out var endTime)) tour.EndTime = endTime;
    }

    /// <summary>
    /// Applies the editable text fields of a translation. Never touches Id, TourId or LanguageCode.
    /// </summary>
    public static void ApplyTo(this UpdateTourTranslationDto dto, TourTranslation translation)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(translation);

        if (dto.Title is { } title) translation.Title = title;
        if (dto.Subtitle is { } subtitle) translation.Subtitle = subtitle;
        if (dto.Description is { } description) translation.Description = description;
        if (dto.DetailedItinerary is { } detailedItinerary) translation.DetailedItinerary = detailedItinerary;
    }

    /// <summary>
    /// Creates a new translation entity. Title and Description are required by the database.
    /// </summary>
    /// <exception cref="ArgumentException">When LanguageCode, Title or Description are missing.</exception>
    public static TourTranslation ToEntity(this UpdateTourTranslationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrWhiteSpace(dto.LanguageCode) || dto.Title is null || dto.Description is null)
        {
            throw new ArgumentException(
                $"New translation '{dto.LanguageCode}' requires LanguageCode, Title, and Description.",
                nameof(dto));
        }

        return new TourTranslation
        {
            LanguageCode = dto.LanguageCode,
            Title = dto.Title,
            Subtitle = dto.Subtitle,
            Description = dto.Description,
            DetailedItinerary = dto.DetailedItinerary
        };
    }

    public static bool TryParseDateTime(string? value, out DateTime result) =>
        DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result);
}
