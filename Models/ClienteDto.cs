namespace ClientesBlazorApp.Models;

/// <summary>
/// Representa un cliente tal como lo devuelve la API (GET).
/// </summary>
public class ClienteDto
{
    public int IdCliente { get; set; }
    public string Cui { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public DateTime FechaNacimiento { get; set; }
}
