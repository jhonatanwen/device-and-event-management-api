using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Eventos;

public sealed class DeleteEventoUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<bool>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var evento = await _unitOfWork.Eventos.GetByIdAsync(id, cancellationToken);

            if (evento is null)
                return Result<bool>.Failure("Evento não encontrado");

            await _unitOfWork.Eventos.DeleteAsync(evento, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Erro ao excluir evento: {ex.Message}");
        }
    }
}
