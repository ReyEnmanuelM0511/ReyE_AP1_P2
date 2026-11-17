using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using ReyE_AP1_P2.Models;
using ReyE_AP1_P2.Context;
using System.Linq.Expressions;

namespace ReyE_AP1_P2.Services;

public class VentasServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Ventas, int>
{
    public async Task<Result> Guardar(Ventas Venta)
    {
        if (!await Existe(Venta.VentaId))
        {
            var inserto = Insertar(Venta);
            return Result.Success(inserto);
        }
        else
        {
            var modifica = Modificar(Venta);
            return Result.Success(modifica);
        }
    }

    public async Task<bool> Existe(int VentaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Venta.AnyAsync(p => p.VentaId == VentaId);
    }

    public async Task<bool> Insertar(Ventas Venta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        foreach (var detalle in Venta.VentaDetalle)
        {
            detalle.venta = null;
        }

        contexto.Add(Venta);
        await AfectarVentas(Venta.VentaDetalle.ToArray(), Operacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Ventas Venta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var entradaAnterior = await contexto.Venta.Include(v => v.VentaDetalle).AsNoTracking().FirstOrDefaultAsync();

        if (entradaAnterior != null)
        {
            await AfectarVentas(entradaAnterior.VentaDetalle.ToArray(), Operacion.Resta);
        }

        await contexto.VentaDetalle.Where(v => v.VentaId == Venta.VentaId).ExecuteDeleteAsync();
        await AfectarVentas(Venta.VentaDetalle.ToArray(), Operacion.Suma);

        foreach (var detalle in Venta.VentaDetalle)
        {
            detalle.Producto = null;
            detalle.VentaId = 0;
        }

        contexto.Update(Venta);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task AfectarVentas(VentasDetalle[] venta, Operacion Operacion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        foreach (var item in venta)
        {
            var ventas = await contexto.Venta.SingleAsync(v => v.VentaId == item.VentaId);
            if (Operacion == Operacion.Suma)
            {
                ventas.Cantidad += item.Cantidad;
                ventas.Producto.Stock -= item.Cantidad;
            }

            else
            {
                ventas.Cantidad -= item.Cantidad;
                ventas.Producto.Stock += item.Cantidad;
            }
        }

        await contexto.SaveChangesAsync();
    }

    public async Task<Result> Eliminar(int VentaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var eliminado = await contexto.Venta.Include(p => p.VentaDetalle).FirstOrDefaultAsync(p => p.VentaId == VentaId);

        if (eliminado == null)
            return Result.Failure(new Error("Pedido.NoEncontrado", "La Venta no fue encontrado"));

        await AfectarVentas(eliminado.VentaDetalle.ToArray(), Operacion.Resta);

        contexto.VentaDetalle.RemoveRange(eliminado.VentaDetalle);
        contexto.Venta.Remove(eliminado);
        var cantidad = await contexto.SaveChangesAsync();
        return Result.Success(eliminado);
    }

    public async Task<Result<Ventas?>> Buscar(int VentaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Venta.Include(v => v.VentaDetalle).ThenInclude(v => v.Producto).FirstOrDefaultAsync(v => v.VentaId == VentaId);
    }

    public async Task<Result<List<Ventas>>> GetList(Expression<Func<Ventas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Venta.Include(p => p.VentaDetalle)
            .ThenInclude(v => v.Producto).Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Ventas>> Listar(Expression<Func<Ventas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Venta.Include(v => v.VentaDetalle).ThenInclude(v => v.Producto).Where(criterio).AsNoTracking().ToListAsync();
    }   

    public enum Operacion
    {
        Suma = 1,
        Resta = 2
    }
}
