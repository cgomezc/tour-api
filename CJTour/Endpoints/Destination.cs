using CJTour.Application.Interfaces;
using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;

namespace CJTour.Endpoints;

public static class Destination
{
    public static WebApplication MapDestinationEndpoints(this WebApplication app)
    {
        var destinationsApi = app.MapGroup("/api/destinations").WithTags("Destinations");

        // GET /api/destinations
        destinationsApi.MapGet("/", GetAllDestinationsAsync)
            .WithName("GetAllDestinations")
            .WithSummary("Get all destinations")
            .WithDescription("Retrieves all destinations filtered by language")
            .Produces<IEnumerable<DestinationDto>>(200)
            .Produces(500)
            .AllowAnonymous()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(3)).Tag("destinations"));

        // GET /api/destinations/{id}
        destinationsApi.MapGet("/{id:int}", GetDestinationByIdAsync)
            .WithName("GetDestinationById")
            .WithSummary("Get destination by ID")
            .WithDescription("Retrieves a specific destination by its ID")
            .Produces<DestinationDto>(200)
            .Produces(404)
            .Produces(500)
            .AllowAnonymous();

        // GET /api/destinations/slug/{slug}
        destinationsApi.MapGet("/slug/{slug}", GetDestinationBySlugAsync)
            .WithName("GetDestinationBySlug")
            .WithSummary("Get destination by slug")
            .WithDescription("Retrieves a specific destination by its slug")
            .Produces<DestinationDto>(200)
            .Produces(404)
            .Produces(500)
            .AllowAnonymous();

        return app;
    }

    private static async Task<IResult> GetAllDestinationsAsync(
        IDestinationService destinationService,
        IBenchmarkService benchmarkService,
        string language = AppConstants.Culture.DefaultLanguage)
    {
        benchmarkService.StartTimer("GetAllDestinations");

        try
        {
            var destinations = await destinationService.GetAllDestinationsAsync(language);
            return Results.Ok(destinations);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving destinations",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer("GetAllDestinations");
        }
    }

    private static async Task<IResult> GetDestinationByIdAsync(
        int id,
        IDestinationService destinationService,
        IBenchmarkService benchmarkService,
        string language = AppConstants.Culture.DefaultLanguage)
    {
        benchmarkService.StartTimer($"GetDestinationById_{id}");

        try
        {
            var destination = await destinationService.GetDestinationByIdAsync(id, language);
            return destination != null ? Results.Ok(destination) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving the destination",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"GetDestinationById_{id}");
        }
    }

    private static async Task<IResult> GetDestinationBySlugAsync(
        string slug,
        IDestinationService destinationService,
        IBenchmarkService benchmarkService,
        string language = AppConstants.Culture.DefaultLanguage)
    {
        benchmarkService.StartTimer($"GetDestinationBySlug_{slug}");

        try
        {
            var destination = await destinationService.GetDestinationBySlugAsync(slug, language);
            return destination != null ? Results.Ok(destination) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving the destination",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"GetDestinationBySlug_{slug}");
        }
    }
}
