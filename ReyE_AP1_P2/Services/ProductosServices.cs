using Aplicada1.Core;
using ReyE_AP1_P2.Models;
using ReyE_AP1_P2.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ReyE_AP1_P2.Services;

public class ProductosServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Productos, int>
{
    public async Task<Result> Guardar(Productos Producto)
    {
        if (!await Existe(Producto.ProductoId))
        {
            var Inserto = Insertar(Producto);
            return Result.Success(Inserto);
        }
        else
        {
            var modifica = Modificar(Producto);
            return Result.Success(modifica);
        }
    }

    public async Task<bool> Existe(int ProductoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Producto.AnyAsync(p => p.ProductoId == ProductoId);
    }

    public async Task<bool> Insertar(Productos Producto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Add(Producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Productos Producto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(Producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int ProductoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = contexto.Producto.Where(p => p.ProductoId == ProductoId).ExecuteDeleteAsync();

        if (contexto.Producto == null)
        {
            return Result.Failure(new Error("Error.NoPudoEliminar", "No se pudo eliminar este elemento"));
        }

        return Result.Success(eliminado);
    }

    public async Task<Result<Productos?>> Buscar(int ProductoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Producto.FirstOrDefaultAsync(p => p.ProductoId == ProductoId);
    }

    public async Task<Result<List<Productos>>> GetList(Expression<Func<Productos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Producto.Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<List<Productos>> Listar(Expression<Func<Productos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Producto.Where(criterio).AsNoTracking().ToListAsync();
    }
}

