using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Eventos;

public sealed class GetEventosByDispositivoUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<Result<IEnumerable<EventoResponse>>> ExecuteAsync(Guid dispositivoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var dispositivo = await _unitOfWork.Dispositivos.GetByIdAsync(dispositivoId, cancellationToken);

            if (dispositivo is null)
                return Result<IEnumerable<EventoResponse>>.Failure(ErrorMessages.NotFound.Dispositivo);

            var eventos = await _unitOfWork.Eventos.GetByDispositivoIdAsync(dispositivoId, cancellationToken);

            var response = eventos.Select(evento => new EventoResponse(
                evento.Id,
                evento.Tipo,
                evento.DataHora,
                evento.DispositivoId
            ));

            return Result<IEnumerable<EventoResponse>>.Success(response);
        }
        catch (Exception)
        {
            return Result<IEnumerable<EventoResponse>>.Failure(ErrorMessages.Internal.GetEntity("eventos"));
        }
    }
}
