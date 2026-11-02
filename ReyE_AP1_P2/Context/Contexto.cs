using Microsoft.EntityFrameworkCore;
using ReyE_AP1_P2.Models;

namespace ReyE_AP1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }
}

