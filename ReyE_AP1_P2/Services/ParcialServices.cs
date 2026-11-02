using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using ReyE_AP1_P2.Models;
using ReyE_AP1_P2.Context;
using System.Linq.Expressions;

namespace ReyE_AP1_P2.Services;

public class ParcialServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Modelo, int>
{

}

