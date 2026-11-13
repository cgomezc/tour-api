using CJTour.Application.Mappings;
using CJTour.Contracts.DTOs;
using CJTour.Domain.Entities;
using Xunit;

namespace CJTour.Application.Tests;

public class TourUpdateMapperTests
{
    [Fact]
    public void ApplyTo_AppliesNullableDtoValuesToNonNullableEntityProperties()
    {
        var tour = new Tour
        {
            DestinationId = 1,
            DurationHours = 2m,
            DurationDays = 1,
            MinParticipants = 1,
            MaxParticipants = 5,
            IsActive = true
        };
        var update = new UpdateTourDto
        {
            DestinationId = 7,
            DurationHours = 5.5m,
            DurationDays = 3,
            MinParticipants = 2,
            MaxParticipants = 20,
            IsActive = false
        };

        update.ApplyTo(tour);

        Assert.Equal(7, tour.DestinationId);
        Assert.Equal(5.5m, tour.DurationHours);
        Assert.Equal(3, tour.DurationDays);
        Assert.Equal(2, tour.MinParticipants);
        Assert.Equal(20, tour.MaxParticipants);
        Assert.False(tour.IsActive);
    }

    [Fact]
    public void ApplyTo_PreservesValuesWhenDtoFieldsAreNull()
    {
        var tour = new Tour
        {
            Slug = "original",
            DurationHours = 4m,
            MaxParticipants = 10,
            IsActive = true,
            TourCode = "T-1",
            AvailableDays = [1, 2]
        };

        new UpdateTourDto().ApplyTo(tour);

        Assert.Equal("original", tour.Slug);
        Assert.Equal(4m, tour.DurationHours);
        Assert.Equal(10, tour.MaxParticipants);
        Assert.True(tour.IsActive);
        Assert.Equal("T-1", tour.TourCode);
        Assert.Equal([1, 2], tour.AvailableDays);
    }

    [Fact]
    public void ApplyTo_ParsesIsoStartAndEndTime()
    {
        var tour = new Tour();
        var update = new UpdateTourDto
        {
            StartTime = "2026-11-10T08:30:00",
            EndTime = "not-a-date"
        };

        update.ApplyTo(tour);

        Assert.Equal(new DateTime(2026, 11, 10, 8, 30, 0), tour.StartTime);
        Assert.Null(tour.EndTime);
    }

    [Fact]
    public void ApplyTo_ReplacesGalleryWithUrlsFromRequestArray()
    {
        var tour = new Tour { GalleryImageUrls = ["https://old.example/image.jpg"] };
        var update = new UpdateTourDto
        {
            GalleryImageUrls = ["https://cdn.example/one.jpg", "https://cdn.example/two.jpg"]
        };

        update.ApplyTo(tour);

        Assert.IsType<List<string>>(tour.GalleryImageUrls);
        Assert.Equal(update.GalleryImageUrls, tour.GalleryImageUrls);
    }

    [Fact]
    public void ApplyTo_ReplacesGalleryWithEmptyListWhenRequestArrayIsEmpty()
    {
        var tour = new Tour { GalleryImageUrls = ["https://old.example/image.jpg"] };

        new UpdateTourDto { GalleryImageUrls = [] }.ApplyTo(tour);

        Assert.NotNull(tour.GalleryImageUrls);
        Assert.Empty(tour.GalleryImageUrls);
    }

    [Fact]
    public void ApplyTo_PreservesGalleryWhenRequestValueIsNull()
    {
        var originalGallery = new List<string> { "https://old.example/image.jpg" };
        var tour = new Tour { GalleryImageUrls = originalGallery };

        new UpdateTourDto().ApplyTo(tour);

        Assert.Same(originalGallery, tour.GalleryImageUrls);
    }

    [Fact]
    public void TranslationApplyTo_UpdatesTextButNeverKeysOrLanguage()
    {
        var translation = new TourTranslation
        {
            Id = 42,
            TourId = 9,
            LanguageCode = "en-US",
            Title = "Old",
            Description = "Old description",
            Subtitle = "Keep me"
        };
        var update = new UpdateTourTranslationDto
        {
            Id = 999,
            LanguageCode = "es-MX",
            Title = "New",
            Description = "New description"
        };

        update.ApplyTo(translation);

        Assert.Equal(42, translation.Id);
        Assert.Equal(9, translation.TourId);
        Assert.Equal("en-US", translation.LanguageCode);
        Assert.Equal("New", translation.Title);
        Assert.Equal("New description", translation.Description);
        Assert.Equal("Keep me", translation.Subtitle);
    }

    [Fact]
    public void TranslationToEntity_ThrowsWhenRequiredFieldsAreMissing()
    {
        var dto = new UpdateTourTranslationDto { LanguageCode = "es-MX", Title = "Solo título" };

        Assert.Throws<ArgumentException>(() => dto.ToEntity());
    }

    [Fact]
    public void TranslationToEntity_CreatesEntityWithoutId()
    {
        var dto = new UpdateTourTranslationDto
        {
            Id = 5,
            LanguageCode = "es-MX",
            Title = "Título",
            Description = "Descripción"
        };

        var entity = dto.ToEntity();

        Assert.Equal(0, entity.Id);
        Assert.Equal("es-MX", entity.LanguageCode);
        Assert.Equal("Título", entity.Title);
        Assert.Equal("Descripción", entity.Description);
    }
}
