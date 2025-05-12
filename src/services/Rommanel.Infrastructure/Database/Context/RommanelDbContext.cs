using DevPack.Data.EFCore.Contexts;
using DevPack.Data.EFCore.Outbox;
using DevPack.Domain.Mediator;
using Microsoft.EntityFrameworkCore;
using Rommanel.Domain.Clientes;

namespace Rommanel.Infrastructure.Database.Context;

public class RommanelDbContext(
    DbContextOptions<RommanelDbContext> options,
    IMediatorHandler mediator) : EFDbContextWithMediator<RommanelDbContext>(options, mediator)
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<PessoaFisica> PessoasFisicas { get; set; }
    public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Default);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RommanelDbContext).Assembly);
        
        modelBuilder.Entity<OutboxMessage>(new OutboxMessageConfiguration().Configure);
        
        base.OnModelCreating(modelBuilder);
    }
}