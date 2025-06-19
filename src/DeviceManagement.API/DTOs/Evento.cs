using DeviceManagement.Domain.Enums;

namespace DeviceManagement.API.DTOs;

public sealed record CreateEvento(
    EventType Tipo,
    Guid DispositivoId,
    DateTime? DataHora = null);

public sealed record Evento(
    Guid Id,
    EventType Tipo,
    DateTime DataHora,
    Guid DispositivoId);
