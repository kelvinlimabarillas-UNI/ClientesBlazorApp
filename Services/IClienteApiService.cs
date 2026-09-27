using ClientesBlazorApp.Models;

namespace ClientesBlazorApp.Services;

public interface IClienteApiService
{
    Task<List<ClienteDto>> ObtenerClientesAsync();
    Task<ClienteDto?> ObtenerClientePorIdAsync(int id);
    Task<ClienteDto> CrearClienteAsync(ClienteFormModel form);
    Task ActualizarClienteAsync(int id, ClienteFormModel form);
    Task EliminarClienteAsync(int id);
}
