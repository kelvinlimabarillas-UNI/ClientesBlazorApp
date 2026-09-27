using System.ComponentModel.DataAnnotations;

namespace ClientesBlazorApp.Models;

/// <summary>
/// Modelo usado por el formulario (modal) de creación/edición.
/// Las reglas de validación se muestran en la interfaz gracias a DataAnnotationsValidator.
/// </summary>
public class ClienteFormModel
{
    [Required(ErrorMessage = "El CUI es obligatorio.")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "El CUI debe tener exactamente 13 dígitos.")]
    public string Cui { get; set; } = string.Empty;

    [Required(ErrorMessage = "El NIT es obligatorio.")]
    [StringLength(15, ErrorMessage = "El NIT no puede tener más de 15 caracteres.")]
    public string Nit { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Direccion { get; set; }

    [StringLength(20)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; }
}
