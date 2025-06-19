namespace DeviceManagement.API.DTOs;

public sealed record CreateDispositivo(
    string Serial,
    string Imei,
    Guid ClienteId,
    DateTime? DataAtivacao = null);

public sealed record UpdateDispositivo(
    string Serial,
    string Imei,
    DateTime? DataAtivacao);

public sealed record PatchDispositivo(
    DateTime? DataAtivacao);

public sealed record Dispositivo(
    Guid Id,
    string Serial,
    string Imei,
    DateTime? DataAtivacao,
    Guid ClienteId);
