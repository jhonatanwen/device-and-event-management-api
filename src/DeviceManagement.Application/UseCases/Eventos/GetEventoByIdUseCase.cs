using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Eventos;

public sealed class GetEventoByIdUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<Result<EventoResponse>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var evento = await _unitOfWork.Eventos.GetByIdAsync(id, cancellationToken);

            if (evento is null)
                return Result<EventoResponse>.Failure(ErrorMessages.NotFound.Evento);

            var response = new EventoResponse(
                evento.Id,
                evento.Tipo,
                evento.DataHora,
                evento.DispositivoId
            );

            return Result<EventoResponse>.Success(response);
        }
        catch (Exception)
        {
            return Result<EventoResponse>.Failure(ErrorMessages.Internal.GetEntity("evento"));
        }
    }
}
