namespace ClientesBlazorApp.Services;

/// <summary>
/// Excepción usada para propagar mensajes de error legibles provenientes de la API
/// (por ejemplo, "Ya existe un cliente con ese CUI").
/// </summary>
public class ApiException : Exception
{
    public ApiException(string message) : base(message)
    {
    }
}
