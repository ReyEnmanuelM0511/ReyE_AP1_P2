using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReyE_AP1_P2.Models;

public class Ventas
{
    [Key]
    public int VentaId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string NombreCliente { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Elija una cantidad valida")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage ="Registre la fecha de la venta")]
    public DateOnly FechaVenta { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Ingrese una cantidad valida")]
    public int Cantidad { get; set; }

    [ForeignKey("ProductoId")]
    public virtual Productos Producto { get; set; } = null!;

    [InverseProperty("venta")]
    public virtual ICollection<VentasDetalle> VentaDetalle { get; set; } = new List<VentasDetalle>();
}

