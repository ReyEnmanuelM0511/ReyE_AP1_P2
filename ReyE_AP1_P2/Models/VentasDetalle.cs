using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReyE_AP1_P2.Models;

public class VentasDetalle
{
    [Key]
    public int DetalleId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Error al completar el campo")]
    public int VentaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Error al completar el campo")]
    public int ProductoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Ingrese una cantidad valida")]
    public int Cantidad { get; set; }

    [Range(1, double.MaxValue, ErrorMessage ="Este campo debe ser completado")]
    public double Precio { get; set; }

    [ForeignKey("ProductoId")]
    public virtual Productos Producto { get; set; } = null!;

    [ForeignKey("VentaId")]
    [InverseProperty("VentaDetalle")]
    public virtual Ventas venta { get; set; } = null!;
}

