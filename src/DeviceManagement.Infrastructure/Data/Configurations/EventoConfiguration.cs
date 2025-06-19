using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManagement.Infrastructure.Data.Configurations;

public sealed class EventoConfiguration : IEntityTypeConfiguration<Evento>
{
    public void Configure(EntityTypeBuilder<Evento> builder)
    {
        builder.ToTable("Eventos");

        builder.HasKey(evento => evento.Id);

        builder.Property(evento => evento.Id)
            .ValueGeneratedNever();

        builder.Property(evento => evento.Tipo)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(evento => evento.DataHora)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(evento => evento.DispositivoId)
            .IsRequired();

        builder.HasIndex(evento => evento.DataHora);
        builder.HasIndex(evento => new { evento.DispositivoId, evento.DataHora });

        builder.HasOne(evento => evento.Dispositivo)
            .WithMany(dispositivo => dispositivo.Eventos)
            .HasForeignKey(evento => evento.DispositivoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
