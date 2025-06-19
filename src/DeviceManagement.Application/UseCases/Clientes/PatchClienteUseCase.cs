using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Clientes;

public sealed record PatchClienteRequest(Guid Id, bool? Status);

public sealed class PatchClienteUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Domain.Entities.Cliente>> ExecuteAsync(
        PatchClienteRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.Id, cancellationToken);            if (cliente is null)
                return Result<Domain.Entities.Cliente>.Failure(ErrorMessages.NotFound.Cliente);

            // Atualizar status se fornecido
            if (request.Status.HasValue)
            {
                if (request.Status.Value)
                {
                    cliente.Ativar();
                }
                else
                {
                    cliente.Desativar();
                }
            }

            await _unitOfWork.Clientes.UpdateAsync(cliente, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Domain.Entities.Cliente>.Success(cliente);
        }        catch (InvalidOperationException ex)
        {
            return Result<Domain.Entities.Cliente>.Failure(ex.Message);
        }
        catch (Exception)
        {
            return Result<Domain.Entities.Cliente>.Failure(ErrorMessages.Internal.UpdateEntity("cliente"));
        }
    }
}
