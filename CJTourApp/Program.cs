
using CJTourApp.Components;
using CJTourApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    //.AddInteractiveWebAssemblyComponents();

// Configurar HttpClient para llamadas a CJTour API
builder.Services.AddHttpClient("CJTourAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7053/"); // Ajusta el puerto de CJTour
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Registrar servicios para consumir la API
builder.Services.AddScoped<ITourApiService, TourApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.UseStaticFiles();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
    //.AddInteractiveWebAssemblyRenderMode()
    //.AddAdditionalAssemblies(typeof(CJTourApp.Client._Imports).Assembly);

app.Run();
