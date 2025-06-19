using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dispositivos;

public sealed class DeleteDispositivoUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<Result<bool>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dispositivo = await _unitOfWork.Dispositivos.GetByIdAsync(id, cancellationToken);

            if (dispositivo is null)
                return Result<bool>.Failure(ErrorMessages.NotFound.Dispositivo);

            // Verificar se existem eventos vinculados
            var eventos = await _unitOfWork.Eventos.GetByDispositivoIdAsync(id, cancellationToken);
            if (eventos.Any())
            {
                return Result<bool>.Failure(ErrorMessages.BusinessRules.DispositivoComEventos);
            }

            await _unitOfWork.Dispositivos.DeleteAsync(dispositivo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(ErrorMessages.Internal.DeleteEntity("dispositivo"));
        }
    }
}
