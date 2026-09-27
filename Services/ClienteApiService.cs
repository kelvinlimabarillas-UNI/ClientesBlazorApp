using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClientesBlazorApp.Models;

namespace ClientesBlazorApp.Services;

/// <summary>
/// Toda la comunicación con la API de clientes pasa por aquí.
/// La aplicación Blazor NUNCA accede directamente a la base de datos: solo usa HttpClient
/// para consumir los endpoints REST (GET, POST, PUT, DELETE) expuestos por ClientesApi.
/// </summary>
public class ClienteApiService : IClienteApiService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ClienteApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ClienteDto>> ObtenerClientesAsync()
    {
        var respuesta = await _httpClient.GetAsync("api/clientes");
        await LanzarSiHayError(respuesta);

        var clientes = await respuesta.Content.ReadFromJsonAsync<List<ClienteDto>>(JsonOptions);
        return clientes ?? new List<ClienteDto>();
    }

    public async Task<ClienteDto?> ObtenerClientePorIdAsync(int id)
    {
        var respuesta = await _httpClient.GetAsync($"api/clientes/{id}");

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await LanzarSiHayError(respuesta);
        return await respuesta.Content.ReadFromJsonAsync<ClienteDto>(JsonOptions);
    }

    public async Task<ClienteDto> CrearClienteAsync(ClienteFormModel form)
    {
        var payload = new
        {
            cui = form.Cui,
            nit = form.Nit,
            nombres = form.Nombres,
            apellidos = form.Apellidos,
            direccion = form.Direccion,
            telefono = form.Telefono,
            fechaNacimiento = form.FechaNacimiento
        };

        var respuesta = await _httpClient.PostAsJsonAsync("api/clientes", payload);
        await LanzarSiHayError(respuesta);

        var creado = await respuesta.Content.ReadFromJsonAsync<ClienteDto>(JsonOptions);
        return creado ?? throw new ApiException("La API no devolvió el cliente creado.");
    }

    public async Task ActualizarClienteAsync(int id, ClienteFormModel form)
    {
        var payload = new
        {
            cui = form.Cui,
            nit = form.Nit,
            nombres = form.Nombres,
            apellidos = form.Apellidos,
            direccion = form.Direccion,
            telefono = form.Telefono,
            fechaNacimiento = form.FechaNacimiento
        };

        var respuesta = await _httpClient.PutAsJsonAsync($"api/clientes/{id}", payload);
        await LanzarSiHayError(respuesta);
    }

    public async Task EliminarClienteAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"api/clientes/{id}");
        await LanzarSiHayError(respuesta);
    }

    /// <summary>
    /// Si la API respondió con un código de error (400, 404, 409, etc.),
    /// intenta leer el mensaje que envió y lo convierte en una excepción legible
    /// para mostrarla en el formulario.
    /// </summary>
    private static async Task LanzarSiHayError(HttpResponseMessage respuesta)
    {
        if (respuesta.IsSuccessStatusCode)
        {
            return;
        }

        string mensaje = $"La API respondió con el código {(int)respuesta.StatusCode}.";

        try
        {
            var contenido = await respuesta.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(contenido))
            {
                using var doc = JsonDocument.Parse(contenido);
                if (doc.RootElement.TryGetProperty("mensaje", out var mensajeProp))
                {
                    mensaje = mensajeProp.GetString() ?? mensaje;
                }
                else if (doc.RootElement.TryGetProperty("title", out var titleProp))
                {
                    mensaje = titleProp.GetString() ?? mensaje;
                }
            }
        }
        catch
        {
            // Si no se pudo interpretar el cuerpo de la respuesta, se usa el mensaje genérico.
        }

        throw new ApiException(mensaje);
    }
}
