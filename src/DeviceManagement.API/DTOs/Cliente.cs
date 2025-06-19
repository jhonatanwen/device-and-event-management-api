namespace DeviceManagement.API.DTOs;

public sealed record CreateCliente(
    string Nome,
    string Email,
    string? Telefone = null,
    bool Status = true);

public sealed record UpdateCliente(
    string Nome,
    string Email,
    string? Telefone);

public sealed record PatchCliente(
    bool? Status);

public sealed record Cliente(
    Guid Id,
    string Nome,
    string Email,
    string? Telefone,
    bool Status);
