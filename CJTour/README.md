# CJTour - Clean Architecture Tour Management System

This project implements a clean architecture pattern for a tour management system based on Prisma models.

## Project Structure

### Core Layer (Domain & Application)
- **Core/Domain/Entities**: Contains all domain entities (Tour, TourTranslation, TourHighlights, etc.)
- **Core/Domain/Repositories**: Repository interfaces defining data access contracts
- **Core/Application/Interfaces**: Service interfaces for business logic
- **Core/Application/Services**: Implementation of business logic services

### Infrastructure Layer
- **Infrastructure/Persistence**: Database context and repository implementations
- **Infrastructure/ExternalServices**: External service integrations

### Presentation Layer
- **Presentation/Controllers**: API controllers handling HTTP requests
- **Presentation/DTOs**: Data Transfer Objects for client responses

## Domain Entities

### Main Entities
- **Tour**: Core tour entity with all tour-related properties
- **TourTranslation**: Multi-language support for tour content
- **TourHighlights**: Tour highlights with translations
- **TourCategory**: Tour categories with hierarchical support
- **TourPackage**: Tour packages with schedules and pricing
- **LanguageOffered**: Languages offered for each tour
- **TourPricing**: Pricing information for tours and packages

### Supporting Entities
- **Destination**: Tour destinations
- **Language**: Supported languages
- **Booking**: Tour bookings
- **CartItem**: Shopping cart items

## API Endpoints

### Tours Controller (`/api/tours`)

#### GET Endpoints
- `GET /api/tours` - Get all tours
- `GET /api/tours/featured` - Get featured tours
- `GET /api/tours/{id}` - Get tour by ID
- `GET /api/tours/slug/{slug}` - Get tour by slug
- `GET /api/tours/category/{categoryId}` - Get tours by category
- `GET /api/tours/destination/{destinationId}` - Get tours by destination
- `GET /api/tours/exists/{id}` - Check if tour exists by ID
- `GET /api/tours/exists/slug/{slug}` - Check if tour exists by slug

#### POST Endpoints
- `POST /api/tours` - Create a new tour

#### PUT Endpoints
- `PUT /api/tours/{id}` - Update an existing tour

#### DELETE Endpoints
- `DELETE /api/tours/{id}` - Delete a tour

### Query Parameters
- `language`: Language code for translations (default: "en")

## DTOs (Data Transfer Objects)

### TourDto
Comprehensive tour information including:
- Basic tour details (ID, slug, duration, etc.)
- Translated content (title, description, etc.)
- Related data (destination, category, highlights, packages, etc.)
- Pricing information
- Language offerings

### Supporting DTOs
- **DestinationDto**: Destination information
- **TourCategoryDto**: Category information with hierarchy
- **TourHighlightDto**: Tour highlights with translations
- **TourPackageDto**: Package information with schedules and pricing
- **LanguageOfferedDto**: Language offerings for tours
- **TourPricingDto**: Pricing information

## Features

### Multi-language Support
- All tour content supports multiple languages
- Language-specific translations for titles, descriptions, highlights, etc.
- Primary language designation for tours

### Tour Management
- Complete CRUD operations for tours
- Featured tour functionality
- Category and destination filtering
- Slug-based routing

### Package System
- Tour packages with multiple schedules
- Package-specific pricing
- Availability tracking
- Display order management

### Pricing System
- Flexible pricing per tour and package
- Multiple currency support
- Different pricing types (per person, per group, etc.)

### Accessibility Features
- Accessibility options tracking
- Minor acceptance settings
- Activity level information
- Required documents tracking

## Clean Architecture Benefits

1. **Separation of Concerns**: Clear separation between domain logic, application services, and presentation
2. **Dependency Inversion**: High-level modules don't depend on low-level modules
3. **Testability**: Easy to unit test business logic in isolation
4. **Maintainability**: Changes in one layer don't affect others
5. **Scalability**: Easy to add new features or modify existing ones

## Getting Started

1. Ensure you have .NET 8.0 installed
2. Clone the repository
3. Install dependencies: `dotnet restore`
4. Configure your database connection in `appsettings.json`
5. Run the application: `dotnet run`

## Dependencies

- **Microsoft.EntityFrameworkCore**: Database ORM
- **Swashbuckle.AspNetCore**: API documentation
- **Microsoft.AspNetCore.Authentication.JwtBearer**: JWT authentication

## Next Steps

1. Implement repository implementations in Infrastructure layer
2. Add Entity Framework DbContext
3. Configure dependency injection in Program.cs
4. Add authentication and authorization
5. Implement validation and error handling
6. Add unit tests for business logic
7. Add integration tests for API endpoints 