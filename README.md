# CJTour API

A .NET Core 8.0 Web API project for tour management system.

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or JetBrains Rider
- SQL Server (if using Entity Framework)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd CJTour
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

## Running the Application

### Using Visual Studio Code

1. Open the project in VS Code
2. Press `F5` or go to Run and Debug panel
3. Select one of the following configurations:
   - **Launch CJTour API (HTTP)** - Runs on http://localhost:5133
   - **Launch CJTour API (HTTPS)** - Runs on https://localhost:7053
   - **Attach to CJTour API** - Attaches to an already running process

### Using Command Line

#### Development Mode
```bash
# HTTP only
dotnet run --project CJTour --urls "http://localhost:5133"

# HTTPS (default)
dotnet run --project CJTour

# Watch mode (auto-restart on file changes)
dotnet watch --project CJTour
```

#### Production Mode
```bash
dotnet run --project CJTour --environment Production
```

### Using Visual Studio

1. Open `CJTour.sln` in Visual Studio
2. Set `CJTour` as the startup project
3. Press `F5` to run with debugging or `Ctrl+F5` to run without debugging

## API Documentation

Once the application is running, you can access:

- **Swagger UI**: https://localhost:7053/swagger (or http://localhost:5133/swagger)
- **API Endpoints**: https://localhost:7053/api/...

## Debugging

### Breakpoints
- Set breakpoints in your C# code
- Use conditional breakpoints for specific scenarios
- Use logpoints for non-intrusive debugging

### Debugging Tips
- Use the Debug Console to evaluate expressions
- Check the Call Stack to understand execution flow
- Use the Watch window to monitor variables
- Use the Immediate window for quick evaluations

## Configuration

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Set to "Development", "Staging", or "Production"
- `ASPNETCORE_URLS`: Override default URLs

### App Settings
- `appsettings.json`: Default configuration
- `appsettings.Development.json`: Development-specific settings
- `appsettings.Production.json`: Production-specific settings

## Project Structure

```
CJTour/
├── Controllers/          # API Controllers
├── Core/                # Domain and Application layers
│   ├── Application/     # Application services and interfaces
│   └── Domain/         # Domain entities and repositories
├── Infrastructure/     # External services and persistence
├── Presentation/       # DTOs and presentation layer
└── Program.cs         # Application entry point
```

## Available Tasks

### VS Code Tasks
- `Ctrl+Shift+P` → "Tasks: Run Task"
- Available tasks:
  - **build**: Build the project
  - **publish**: Publish the project
  - **watch**: Run with file watching
  - **clean**: Clean build artifacts
  - **restore**: Restore NuGet packages
  - **test**: Run tests

### Command Line Tasks
```bash
# Build
dotnet build

# Clean
dotnet clean

# Restore packages
dotnet restore

# Run tests
dotnet test

# Publish
dotnet publish -c Release
```

## Troubleshooting

### Common Issues

1. **Port already in use**
   - Change the port in `launchSettings.json`
   - Or kill the process using the port

2. **SSL Certificate issues**
   - Run `dotnet dev-certs https --trust` to trust the development certificate

3. **Build errors**
   - Clean and rebuild: `dotnet clean && dotnet build`
   - Restore packages: `dotnet restore`

4. **Runtime errors**
   - Check environment variables
   - Verify connection strings
   - Check application logs

### Getting Help

- Check the [.NET documentation](https://docs.microsoft.com/en-us/dotnet/)
- Review the [ASP.NET Core documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- Check the project's issue tracker

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

[Add your license information here] 