using Microsoft.EntityFrameworkCore;
using ReyE_AP1_P2.Models;

namespace ReyE_AP1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Productos> Producto { get; set; }
    public DbSet<Ventas> Venta { get; set; }
    public DbSet<VentasDetalle> VentaDetalle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Productos>().HasData(

            new Productos
            {

                ProductoId = 1,

                NombreProducto = "Mocca",

                Tipo = "Caramelo",

                Precio = 150.0,

                Stock = 10
            },
            new Productos
            {

                ProductoId = 2,

                NombreProducto = "Cappuccino", 
                
                Tipo = "Vainilla",

                Precio = 120.0,

                Stock = 10
            },
            new Productos
            {

                ProductoId = 3,

                NombreProducto = "Latte",

                Tipo = "Helado",

                Precio = 130.0,

                Stock = 10
            },
            new Productos
            {

                ProductoId = 4,

                NombreProducto= "Espresso",

                Tipo = "Doble",

                Precio = 200.0,

                Stock = 10
            },
            new Productos
            {

                ProductoId = 5,

                NombreProducto = "Machiatto",

                Tipo = "Hazelnut",

                Precio = 220.0,

                Stock = 10
            }
        );
    }
}

