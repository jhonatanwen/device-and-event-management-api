using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Eventos;

public sealed class CreateEventoUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<Result<Guid>> ExecuteAsync(CreateEventoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var dispositivo = await _unitOfWork.Dispositivos.GetByIdAsync(request.DispositivoId, cancellationToken);

            if (dispositivo is null)
                return Result<Guid>.Failure(ErrorMessages.NotFound.Dispositivo);

            if (!dispositivo.EstaAtivo)
                return Result<Guid>.Failure(ErrorMessages.BusinessRules.DispositivoInativo);

            var evento = Evento.Create(request.Tipo, request.DispositivoId, request.DataHora);

            await _unitOfWork.Eventos.AddAsync(evento, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(evento.Id);
        }
        catch (ArgumentException ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
        catch (Exception)
        {
            return Result<Guid>.Failure(ErrorMessages.Internal.CreateEntity("evento"));
        }
    }
}

public sealed record CreateEventoRequest(
    EventType Tipo,
    Guid DispositivoId,
    DateTime? DataHora = null);
