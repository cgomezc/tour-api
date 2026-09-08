using CJTour.Application.Extensions;
using CJTour.Contracts.DTOs;
using CJTour.Domain.Entities;
using Xunit;

namespace CJTour.Application.Tests;

public class PatchExtensionsTests
{
    [Fact]
    public void PatchFrom_ReplacesGalleryWithUrlsFromRequestArray()
    {
        var tour = new Tour
        {
            GalleryImageUrls = ["https://old.example/image.jpg"]
        };
        var update = new UpdateTourDto
        {
            GalleryImageUrls =
            [
                "https://cdn.example/one.jpg",
                "https://cdn.example/two.jpg"
            ]
        };

        tour.PatchFrom(update);

        Assert.IsType<List<string>>(tour.GalleryImageUrls);
        Assert.Equal(update.GalleryImageUrls, tour.GalleryImageUrls);
    }

    [Fact]
    public void PatchFrom_ReplacesGalleryWithEmptyListWhenRequestArrayIsEmpty()
    {
        var tour = new Tour
        {
            GalleryImageUrls = ["https://old.example/image.jpg"]
        };
        var update = new UpdateTourDto
        {
            GalleryImageUrls = []
        };

        tour.PatchFrom(update);

        Assert.NotNull(tour.GalleryImageUrls);
        Assert.Empty(tour.GalleryImageUrls);
    }

    [Fact]
    public void PatchFrom_PreservesGalleryWhenRequestValueIsNull()
    {
        var originalGallery = new List<string> { "https://old.example/image.jpg" };
        var tour = new Tour
        {
            GalleryImageUrls = originalGallery
        };
        var update = new UpdateTourDto();

        tour.PatchFrom(update);

        Assert.Same(originalGallery, tour.GalleryImageUrls);
    }
}
