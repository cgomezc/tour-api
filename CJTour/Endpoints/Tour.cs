using CJTour.Application.Interfaces;
using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;

namespace CJTour.Endpoints;

public static class Tour
{
    /// <summary>Output-cache tag shared by all cached tour GET endpoints; evict it after any write.</summary>
    private const string ToursCacheTag = "tours";

    public static WebApplication MapTourEndpoints(this WebApplication app)
    {
        var toursApi = app.MapGroup("/api/tours").WithTags("Tours");

        // POST /api/tours - PROTECTED ENDPOINT
        toursApi.MapPost("/", CreateTourAsync)
            .WithName("CreateTour")
            .WithSummary("Create tour")
            .WithDescription("Creates a new tour and its translations. Requires authentication.")
            .Accepts<CreateTourDto>("application/json")
            .Produces<TourDto>(201)
            .Produces(400)
            .Produces(401)
            .Produces(500);
           // .RequireAuthorization();

        // GET /api/tours
        toursApi.MapGet("/", GetToursAsync)
            .WithName("GetTours")
            .WithSummary("Get all tours")
            .WithDescription("Retrieves all tours with optional language parameter")
            .Produces<IEnumerable<TourDto>>(200)
            .Produces(500)
            .AllowAnonymous()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(2)).Tag(ToursCacheTag));
        // GET /api/tours/featured
        toursApi.MapGet("/featured", GetFeaturedToursAsync)
            .WithName("GetFeaturedTours")
            .WithSummary("Get featured tours")
            .WithDescription("Retrieves featured tours with optional language parameter")
            .Produces<IEnumerable<TourDto>>(200)
            .Produces(500)
            .AllowAnonymous();

        // GET /api/tours/{id}
        toursApi.MapGet("/{id:int}", GetTourByIdAsync)
            .WithName("GetTourById")
            .WithSummary("Get tour by ID")
            .WithDescription("Retrieves a specific tour by its ID")
            .Produces<TourDto>(200)
            .Produces(404)
            .Produces(500)
            .AllowAnonymous()
        .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(5)).Tag(ToursCacheTag));

        // GET /api/tours/slug/{slug}
        toursApi.MapGet("/slug/{slug}", GetTourBySlugAsync)
            .WithName("GetTourBySlug")
            .WithSummary("Get tour by slug")
            .WithDescription("Retrieves a specific tour by its slug")
            .Produces<TourDto>(200)
            .Produces(404)
            .Produces(500)
            .AllowAnonymous();

        // GET /api/tours/destination/{destinationId}
        toursApi.MapGet("/destination/{destinationId:int}", GetToursByDestinationAsync)
            .WithName("GetToursByDestination")
            .WithSummary("Get tours by destination")
            .WithDescription("Retrieves tours filtered by destination ID")
            .Produces<IEnumerable<TourDto>>(200)
            .Produces(500)
            .AllowAnonymous();

        // PUT /api/tours/{id} - PROTECTED ENDPOINT
        toursApi.MapPut("/{id:int}", UpdateTourAsync)
            .WithName("UpdateTour")
            .WithSummary("Update tour")
            .WithDescription("Updates a tour and its translations using the tour ID from the route. Accepts a lightweight UpdateTourDto payload.")
            .Accepts<UpdateTourDto>("application/json")
            .Produces<TourDto>(200)
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500);
        // .RequireAuthorization();

        // DELETE /api/tours/{id} - PROTECTED ENDPOINT
        toursApi.MapDelete("/{id:int}", DeleteTourAsync)
            .WithName("DeleteTour")
            .WithSummary("Delete tour")
            .WithDescription("Deletes a tour by its ID")
            .Produces(204)
            .Produces(401)
            .Produces(500)
            .RequireAuthorization();

        // GET /api/tours/exists/{id}
        toursApi.MapGet("/exists/{id:int}", TourExistsAsync)
            .WithName("TourExists")
            .WithSummary("Check if tour exists")
            .WithDescription("Checks if a tour exists by its ID")
            .Produces<bool>(200)
            .Produces(500)
            .AllowAnonymous();

        // GET /api/tours/exists/slug/{slug}
        toursApi.MapGet("/exists/slug/{slug}", TourExistsBySlugAsync)
            .WithName("TourExistsBySlug")
            .WithSummary("Check if tour exists by slug")
            .WithDescription("Checks if a tour exists by its slug")
            .Produces<bool>(200)
            .Produces(500)
            .AllowAnonymous();

        // GET /api/tours/my-tours - PROTECTED ENDPOINT
        toursApi.MapGet("/my-tours", GetMyToursAsync)
            .WithName("GetMyTours")
            .WithSummary("Get user's tours")
            .WithDescription("Retrieves tours for the authenticated user")
            .Produces<IEnumerable<TourDto>>(200)
            .Produces(401)
            .Produces(500)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetToursAsync(
        ITourService tourService, 
        IBenchmarkService benchmarkService, 
        string language = AppConstants.Culture.DefaultLanguage)
    {
        benchmarkService.StartTimer("GetTours");
        
        try
        {
            // Caching is handled by the output cache (tag "tours"), which is evicted on every write.
            var tours = await tourService.GetAllToursAsync(language, TourIncludeOptions.Translations);
            return Results.Ok(tours);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving tours",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer("GetTours");
        }
    }

    private static async Task<IResult> GetFeaturedToursAsync(
        ITourService tourService, 
        IBenchmarkService benchmarkService, 
        string language = "en")
    {
        benchmarkService.StartTimer("GetFeaturedTours");
        
        try
        {
            var tours = await tourService.GetFeaturedToursAsync(language);
            return Results.Ok(tours);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving featured tours",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer("GetFeaturedTours");
        }
    }

    private static async Task<IResult> GetTourByIdAsync(
        int id, 
        ITourService tourService, 
        IBenchmarkService benchmarkService, 
        string language = "en")
    {
        benchmarkService.StartTimer($"GetTourById_{id}");
        
        try
        {
            var tour = await tourService.GetTourByIdAsync(id, language, TourIncludeOptions.Translations);
            if (tour == null)
                return Results.NotFound(new { message = $"Tour with ID {id} not found" });

            return Results.Ok(tour);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving the tour",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"GetTourById_{id}");
        }
    }

    private static async Task<IResult> GetTourBySlugAsync(
        string slug, 
        ITourService tourService, 
        IBenchmarkService benchmarkService, 
        string language = "en")
    {
        benchmarkService.StartTimer($"GetTourBySlug_{slug}");
        
        try
        {
            var tour = await tourService.GetTourBySlugAsync(slug, language);
            if (tour == null)
                return Results.NotFound(new { message = $"Tour with slug '{slug}' not found" });

            return Results.Ok(tour);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving the tour",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"GetTourBySlug_{slug}");
        }
    }

    

    private static async Task<IResult> GetToursByDestinationAsync(
        int destinationId, 
        ITourService tourService, 
        IBenchmarkService benchmarkService, 
        string language = "en")
    {
        benchmarkService.StartTimer($"GetToursByDestination_{destinationId}");
        
        try
        {
            var tours = await tourService.GetToursByDestinationAsync(destinationId, language);
            return Results.Ok(tours);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving tours by destination",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"GetToursByDestination_{destinationId}");
        }
    }

    private static async Task<IResult> CreateTourAsync(
        CreateTourDto createDto,
        ITourService tourService,
        IBenchmarkService benchmarkService,
        IOutputCacheStore outputCache,
        HttpContext context,
        CancellationToken ct,
        string language = AppConstants.Culture.DefaultLanguage)
    {
        benchmarkService.StartTimer("CreateTour");

        try
        {
            var userId = "gurunator";
            var createdTour = await tourService.CreateTourAsync(createDto, userId, language);

            // Invalidate every cached tour response (all languages, lists and details)
            await outputCache.EvictByTagAsync(ToursCacheTag, ct);

            return Results.Created($"/api/tours/{createdTour.Id}", createdTour);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while creating the tour",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer("CreateTour");
        }
    }

    private static async Task<IResult> UpdateTourAsync(
        int id,
        UpdateTourDto updateDto,
        ITourService tourService,
        IBenchmarkService benchmarkService,
        IOutputCacheStore outputCache,
        CancellationToken ct,
        string language = AppConstants.Culture.DefaultLanguage)
    {
        benchmarkService.StartTimer($"UpdateTour_{id}");

        try
        {     

            var updatedTour = await tourService.UpdateTourAsync(id, updateDto, language);
            if (updatedTour == null)
                return Results.NotFound(new { message = $"Tour with ID {id} not found" });

            // Invalidate every cached tour response (all languages, lists and details)
            await outputCache.EvictByTagAsync(ToursCacheTag, ct);

            return Results.Ok(updatedTour);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while updating the tour",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"UpdateTour_{id}");
        }
    }

    private static async Task<IResult> DeleteTourAsync(
        int id, 
        ITourService tourService, 
        IBenchmarkService benchmarkService,
        IOutputCacheStore outputCache,
        HttpContext context,
        CancellationToken ct)
    {
        benchmarkService.StartTimer($"DeleteTour_{id}");
        
        try
        {
            // Check if user is authenticated
            var userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            await tourService.DeleteTourAsync(id);
            await outputCache.EvictByTagAsync(ToursCacheTag, ct);
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while deleting the tour",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"DeleteTour_{id}");
        }
    }

    private static async Task<IResult> TourExistsAsync(
        int id, 
        ITourService tourService, 
        IBenchmarkService benchmarkService)
    {
        benchmarkService.StartTimer($"TourExists_{id}");
        
        try
        {
            var exists = await tourService.TourExistsAsync(id);
            return Results.Ok(exists);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while checking tour existence",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"TourExists_{id}");
        }
    }

    private static async Task<IResult> TourExistsBySlugAsync(
        string slug, 
        ITourService tourService, 
        IBenchmarkService benchmarkService)
    {
        benchmarkService.StartTimer($"TourExistsBySlug_{slug}");
        
        try
        {
            var exists = await tourService.TourExistsBySlugAsync(slug);
            return Results.Ok(exists);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while checking tour existence",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer($"TourExistsBySlug_{slug}");
        }
    }

    private static async Task<IResult> GetMyToursAsync(
        ITourService tourService, 
        IBenchmarkService benchmarkService,
        HttpContext context,
        string language = "en")
    {
        benchmarkService.StartTimer("GetMyTours");
        
        try
        {
            var userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            // For now, just return all tours (you can implement user-specific logic later)
            var tours = await tourService.GetAllToursAsync(language);
            return Results.Ok(tours);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while retrieving user tours",
                detail: ex.Message
            );
        }
        finally
        {
            benchmarkService.StopTimer("GetMyTours");
        }
    }
}