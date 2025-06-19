using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Eventos;

public sealed class GetAllEventosUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<IEnumerable<EventoResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var eventos = await _unitOfWork.Eventos.GetAllAsync(cancellationToken);

            var response = eventos.Select(evento => new EventoResponse(
                evento.Id,
                evento.Tipo,
                evento.DataHora,
                evento.DispositivoId
            ));

            return Result<IEnumerable<EventoResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EventoResponse>>.Failure($"Erro ao buscar eventos: {ex.Message}");
        }
    }
}
