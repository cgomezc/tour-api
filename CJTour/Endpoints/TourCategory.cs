using CJTour.Application.Interfaces;
using CJTour.Contracts.DTOs;

namespace CJTour.Endpoints
{
    public static class TourCategory
    {
        public static WebApplication MapTourCategoryEndpoints(this WebApplication app)
        {
            var toursApi = app.MapGroup("/api/categories").WithTags("Categories");
            // GET /api/categories
            toursApi.MapGet("/", GetAllTourCategories)
                .WithName("GetAllTourCategories")
                .WithSummary("Get all tour categories")
                .WithDescription("Retrieves all tour categories")
                .Produces<IEnumerable<TourCategoryDto>>(200)
                .Produces(500)
                .AllowAnonymous();

            return app;
        }

        private static async Task<IResult> GetAllTourCategories(
            ICategoryService categoryService,
            IBenchmarkService benchmarkService)
        {
            benchmarkService.StartTimer("GetToursByCategory");

            try
            {
                var tours = await  categoryService.GetAllCategoriesAsync();
                return Results.Ok(tours);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    statusCode: 500,
                    title: "An error occurred while retrieving tours by category",
                    detail: ex.Message
                );
            }
            finally
            {
                benchmarkService.StopTimer("GetToursByCategory");
            }
        }
    }
}