using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Enums;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Eventos;

public sealed class GetEventosByPeriodUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<Result<IEnumerable<EventoResponse>>> ExecuteAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            if (startDate > endDate)
                return Result<IEnumerable<EventoResponse>>.Failure(ErrorMessages.BusinessRules.DataInicialMaiorQueFinal);

            var eventos = await _unitOfWork.Eventos.GetByPeriodAsync(startDate, endDate, cancellationToken);

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

public sealed record EventoResponse(
    Guid Id,
    EventType Tipo,
    DateTime DataHora,
    Guid DispositivoId);
