public static class ToursEndpoints
{
    public static void Map(WebApplication app)
    {
        // Example: Get all tours (requires authentication)
        app.MapGet("/tours", [Microsoft.AspNetCore.Authorization.Authorize]() =>
        {
            // Replace with your actual data access logic
            return Results.Ok(new[] { "Tour1", "Tour2" });
        });

        // Example: Create a new tour (requires authentication)
        app.MapPost("/tours", [Microsoft.AspNetCore.Authorization.Authorize]() =>
        {
            // Replace with your actual creation logic
            return Results.Created("/tours/1", new { Id = 1, Name = "Tour1" });
        });
    }
}