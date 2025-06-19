using DeviceManagement.Domain.Common;
using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Domain.Entities;

public sealed class Evento : BaseEntity
{
    public EventType Tipo { get; private set; }
    public DateTime DataHora { get; private set; }
    public Guid DispositivoId { get; private set; }    public Dispositivo Dispositivo { get; private set; }

    private Evento()
    {
        Dispositivo = null!;
    }

    private Evento(EventType tipo, DateTime dataHora, Guid dispositivoId)
    {
        Tipo = tipo;
        DataHora = dataHora;
        DispositivoId = dispositivoId;
        Dispositivo = null!;
    }

    public static Evento Create(EventType tipo, Guid dispositivoId, DateTime? dataHora = null)
    {
        var eventDateTime = dataHora ?? DateTime.UtcNow;

        if (eventDateTime > DateTime.UtcNow)
            throw new ArgumentException("Data do evento não pode ser no futuro", nameof(dataHora));

        return new Evento(tipo, eventDateTime, dispositivoId);
    }

    public void UpdateTipo(EventType tipo)
    {
        Tipo = tipo;
    }

    public void UpdateDataHora(DateTime dataHora)
    {
        if (dataHora > DateTime.UtcNow)
            throw new ArgumentException("Data do evento não pode ser no futuro", nameof(dataHora));

        DataHora = dataHora;
    }
}
