using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Clientes;

public sealed class DeleteClienteUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);

            if (cliente is null)
                return Result.Failure("Cliente não encontrado");

            var dispositivos = await _unitOfWork.Dispositivos.GetByClienteIdAsync(id, cancellationToken);

            if (dispositivos.Any())
                return Result.Failure("Não é possível excluir cliente que possui dispositivos");

            await _unitOfWork.Clientes.DeleteAsync(cliente, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro interno: {ex.Message}");
        }
    }
}
