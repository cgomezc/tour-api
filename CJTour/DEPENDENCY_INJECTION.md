# Dependency Injection Configuration

This document describes the dependency injection (DI) configuration for the CJTour API, including service registrations, repository patterns, and application architecture.

## Overview

The DI configuration follows Clean Architecture principles with proper separation of concerns:
- **Core Layer**: Domain models, interfaces, and application services
- **Infrastructure Layer**: Repository implementations, database context, and external services
- **Presentation Layer**: Controllers and DTOs

## Service Registration Structure

### 1. Infrastructure Services
**File**: `Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`

#### Database Context
```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
```

#### Repositories
```csharp
services.AddScoped<ITourRepository, TourRepository>();
services.AddScoped<ITourPackageRepository, TourPackageRepository>();
```

#### Filters
```csharp
services.AddValidationFilter();
```

### 2. Application Services
**File**: `Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`

#### Business Logic Services
```csharp
services.AddScoped<ITourService, TourService>();
```

### 3. Cross-Cutting Concerns
**File**: `Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`

#### CORS Policy
```csharp
services.AddCorsPolicy(configuration);
```

#### Authorization Policies
```csharp
services.AddAuthorizationPolicies();
```

## Service Lifetimes

### Scoped Services (Per HTTP Request)
- **Repositories**: `ITourRepository`, `ITourPackageRepository`
- **Application Services**: `ITourService`
- **Database Context**: `AppDbContext`

### Singleton Services
- **Configuration**: `IConfiguration`
- **Logging**: `ILogger<T>`

### Transient Services
- **Filters**: `ValidationFilter`

## Repository Pattern Implementation

### Repository Interfaces
**Location**: `Core/Domain/Repositories/`

#### ITourRepository
```csharp
public interface ITourRepository
{
    Task<IEnumerable<Tour>> GetAllAsync();
    Task<Tour?> GetByIdAsync(int tourId);
    Task<Tour?> GetBySlugAsync(string slug);
    Task<Tour?> GetByIdWithDetailsAsync(int tourId, string languageCode = "en");
    Task<IEnumerable<Tour>> GetFeaturedToursAsync(string languageCode = "en");
    Task<IEnumerable<Tour>> GetToursByCategoryAsync(int categoryId, string languageCode = "en");
    Task<IEnumerable<Tour>> GetToursByDestinationAsync(int destinationId, string languageCode = "en");
    Task<Tour> AddAsync(Tour tour);
    Task<Tour> UpdateAsync(Tour tour);
    Task DeleteAsync(int tourId);
    Task<bool> ExistsAsync(int tourId);
    Task<bool> ExistsBySlugAsync(string slug);
}
```

#### ITourPackageRepository
```csharp
public interface ITourPackageRepository
{
    Task<IEnumerable<TourPackage>> GetByTourIdAsync(int tourId);
    Task<TourPackage?> GetByIdAsync(int packageId);
    Task<TourPackage?> GetByIdWithDetailsAsync(int packageId, string languageCode = "en");
    Task<TourPackage> AddAsync(TourPackage tourPackage);
    Task<TourPackage> UpdateAsync(TourPackage tourPackage);
    Task DeleteAsync(int packageId);
    Task<bool> ExistsAsync(int packageId);
}
```

### Repository Implementations
**Location**: `Infrastructure/Persistence/Repositories/`

#### TourRepository
- Implements `ITourRepository`
- Uses Entity Framework Core for data access
- Includes proper navigation property loading
- Implements soft delete pattern

#### TourPackageRepository
- Implements `ITourPackageRepository`
- Handles tour package-specific operations
- Includes translation and schedule loading

## Application Services

### Service Interfaces
**Location**: `Core/Application/Interfaces/`

#### ITourService
```csharp
public interface ITourService
{
    Task<IEnumerable<TourDto>> GetAllToursAsync(string languageCode = "en");
    Task<TourDto?> GetTourByIdAsync(int tourId, string languageCode = "en");
    Task<TourDto?> GetTourBySlugAsync(string slug, string languageCode = "en");
    Task<IEnumerable<TourDto>> GetFeaturedToursAsync(string languageCode = "en");
    Task<IEnumerable<TourDto>> GetToursByCategoryAsync(int categoryId, string languageCode = "en");
    Task<IEnumerable<TourDto>> GetToursByDestinationAsync(int destinationId, string languageCode = "en");
    Task DeleteTourAsync(int tourId);
    Task<bool> TourExistsAsync(int tourId);
    Task<bool> TourExistsBySlugAsync(string slug);
}
```

### Service Implementations
**Location**: `Core/Application/Services/`

#### TourService
- Implements `ITourService`
- Orchestrates business logic
- Maps domain models to DTOs
- Handles language-specific content

## Program.cs Configuration

### Service Registration Order
1. **Infrastructure Services** (Database, Repositories, Filters)
2. **Application Services** (Business Logic)
3. **Cross-Cutting Concerns** (CORS, Authorization)
4. **Authentication/Authorization** (JWT)

### Example Usage
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add infrastructure services (database, repositories, filters)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add application services
builder.Services.AddApplicationServices();

// Add CORS policy
builder.Services.AddCorsPolicy(builder.Configuration);

// Add authorization policies
builder.Services.AddAuthorizationPolicies();
```

## Adding New Services

### 1. Create Interface
```csharp
// Core/Application/Interfaces/INewService.cs
public interface INewService
{
    Task<NewDto> GetNewAsync(int id);
}
```

### 2. Create Implementation
```csharp
// Core/Application/Services/NewService.cs
public class NewService : INewService
{
    private readonly INewRepository _newRepository;

    public NewService(INewRepository newRepository)
    {
        _newRepository = newRepository;
    }

    public async Task<NewDto> GetNewAsync(int id)
    {
        // Implementation
    }
}
```

### 3. Register in DI
```csharp
// Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs
public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    services.AddScoped<ITourService, TourService>();
    services.AddScoped<INewService, NewService>(); // Add this line
    
    return services;
}
```

## Testing DI Configuration

### Health Check
```bash
GET /health
```

### API Status
```bash
GET /api/status
```

### Test Controller Injection
```csharp
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ITourService _tourService;

    public TestController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        var tours = await _tourService.GetAllToursAsync();
        return Ok(tours);
    }
}
```

## Best Practices

### 1. Interface Segregation
- Keep interfaces focused and specific
- Avoid large, monolithic interfaces

### 2. Dependency Inversion
- Depend on abstractions, not concretions
- Use interfaces for all external dependencies

### 3. Service Lifetime Management
- Use `Scoped` for database contexts and repositories
- Use `Singleton` for configuration and logging
- Use `Transient` for lightweight, stateless services

### 4. Constructor Injection
- Prefer constructor injection over property injection
- Keep constructors simple and focused

### 5. Error Handling
- Use the global exception handler middleware
- Log errors appropriately
- Return consistent error responses

## Troubleshooting

### Common Issues

1. **Circular Dependencies**
   - Review service dependencies
   - Consider using events or mediator pattern

2. **Service Not Registered**
   - Check if service is registered in appropriate extension method
   - Verify namespace imports

3. **Scoped Service in Singleton**
   - Ensure proper lifetime management
   - Use service locator pattern if needed

4. **Database Context Issues**
   - Verify connection string configuration
   - Check Entity Framework migrations

### Debugging Tips

1. **Use Built-in DI Container Features**
   ```csharp
   var services = app.Services;
   var tourService = services.GetService<ITourService>();
   ```

2. **Check Service Registration**
   ```csharp
   var descriptors = services.Where(d => d.ServiceType == typeof(ITourService));
   ```

3. **Use Logging**
   ```csharp
   _logger.LogInformation("Service {ServiceType} registered", typeof(ITourService));
   ```

## Configuration Files

### appsettings.Development.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=cjtour;Username=postgres;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "CJTour": "Debug"
    }
  }
}
```

### appsettings.Production.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-production-host;Database=cjtour;Username=your_username;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "CJTour": "Information"
    }
  }
}
``` 