using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReyE_AP1_P2.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? NombreProducto { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Tipo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar una cantidad valida")]
    public int Stock { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Debe ingresar un precio valido")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public bool Estado { get; set; } = true;
}

