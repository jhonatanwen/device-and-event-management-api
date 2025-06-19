using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dispositivos;

public sealed class GetDispositivoByIdUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<Result<DispositivoResponse>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dispositivo = await _unitOfWork.Dispositivos.GetByIdAsync(id, cancellationToken);

            if (dispositivo is null)
                return Result<DispositivoResponse>.Failure(ErrorMessages.NotFound.Dispositivo);

            var response = new DispositivoResponse(
                dispositivo.Id,
                dispositivo.Serial,
                dispositivo.Imei,
                dispositivo.DataAtivacao,
                dispositivo.ClienteId
            );

            return Result<DispositivoResponse>.Success(response);
        }
        catch (Exception)
        {
            return Result<DispositivoResponse>.Failure(ErrorMessages.Internal.GetEntity("dispositivo"));
        }
    }
}

public sealed record DispositivoResponse(
    Guid Id,
    string Serial,
    string Imei,
    DateTime? DataAtivacao,
    Guid ClienteId);
