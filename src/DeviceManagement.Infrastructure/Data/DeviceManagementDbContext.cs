using DeviceManagement.Domain.Entities;
using DeviceManagement.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Data;

public sealed class DeviceManagementDbContext(DbContextOptions<DeviceManagementDbContext> options) : DbContext(options)
{
  public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Dispositivo> Dispositivos => Set<Dispositivo>();
    public DbSet<Evento> Eventos => Set<Evento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        modelBuilder.ApplyConfiguration(new DispositivoConfiguration());
        modelBuilder.ApplyConfiguration(new EventoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
