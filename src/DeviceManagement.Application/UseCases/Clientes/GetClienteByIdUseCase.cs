using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Clientes;

public sealed class GetClienteByIdUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<ClienteResponse>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);            if (cliente is null)
                return Result<ClienteResponse>.Failure(ErrorMessages.NotFound.Cliente);

            var response = new ClienteResponse(
                cliente.Id,
                cliente.Nome,
                cliente.Email,
                cliente.Telefone,
                cliente.Status);

            return Result<ClienteResponse>.Success(response);
        }        catch (Exception)
        {
            return Result<ClienteResponse>.Failure(ErrorMessages.Internal.GetEntity("cliente"));
        }
    }
}

public sealed record ClienteResponse(
    Guid Id,
    string Nome,
    string Email,
    string? Telefone,
    bool Status);
