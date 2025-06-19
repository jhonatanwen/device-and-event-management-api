using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManagement.Infrastructure.Data.Configurations;

public sealed class DispositivoConfiguration : IEntityTypeConfiguration<Dispositivo>
{
    public void Configure(EntityTypeBuilder<Dispositivo> builder)
    {
        builder.ToTable("Dispositivos");

        builder.HasKey(dispositivo => dispositivo.Id);

        builder.Property(dispositivo => dispositivo.Id)
            .ValueGeneratedNever();

        builder.Property(dispositivo => dispositivo.Serial)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dispositivo => dispositivo.Imei)
            .IsRequired()
            .HasMaxLength(15)
            .HasConversion(
                imei => imei.Value,
                value => Imei.Create(value));

        builder.Property(dispositivo => dispositivo.DataAtivacao);

        builder.Property(dispositivo => dispositivo.ClienteId)
            .IsRequired();

        builder.HasIndex(dispositivo => dispositivo.Serial)
            .IsUnique();

        builder.HasIndex(dispositivo => dispositivo.Imei)
            .IsUnique();

        builder.HasOne(dispositivo => dispositivo.Cliente)
            .WithMany(c => c.Dispositivos)
            .HasForeignKey(dispositivo => dispositivo.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(dispositivo => dispositivo.Eventos)
            .WithOne(e => e.Dispositivo)
            .HasForeignKey(e => e.DispositivoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
