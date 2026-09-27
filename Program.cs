using ClientesBlazorApp.Components;
using ClientesBlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios de Blazor (componentes interactivos en el servidor)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// URL base de la API de clientes (ClientesApi). Se lee de appsettings.json.
var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("No se encontró la configuración 'ApiBaseUrl' en appsettings.json.");

// Este HttpClient es la ÚNICA forma en que la app habla con los datos de clientes:
// nunca se usa Entity Framework ni se toca la base de datos desde aquí.
builder.Services.AddHttpClient<IClienteApiService, ClienteApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
