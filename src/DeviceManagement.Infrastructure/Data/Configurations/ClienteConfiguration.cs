using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManagement.Infrastructure.Data.Configurations;

public sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(cliente => cliente.Id);

        builder.Property(cliente => cliente.Id)
            .ValueGeneratedNever();

        builder.Property(cliente => cliente.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cliente => cliente.Email)
            .IsRequired()
            .HasMaxLength(254)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.Property(cliente => cliente.Telefone)
            .HasMaxLength(20);

        builder.Property(cliente => cliente.Status)
            .IsRequired();

        builder.HasIndex(cliente => cliente.Email)
            .IsUnique();

        builder.HasMany(cliente => cliente.Dispositivos)
            .WithOne(dispositivo => dispositivo.Cliente)
            .HasForeignKey(dispositivo => dispositivo.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
