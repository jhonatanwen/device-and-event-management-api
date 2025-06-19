using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dispositivos;

public sealed class GetAllDispositivosUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<IEnumerable<DispositivoResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var dispositivos = await _unitOfWork.Dispositivos.GetAllAsync(cancellationToken);

            var response = dispositivos.Select(dispositivo => new DispositivoResponse(
                dispositivo.Id,
                dispositivo.Serial,
                dispositivo.Imei,
                dispositivo.DataAtivacao,
                dispositivo.ClienteId
            ));

            return Result<IEnumerable<DispositivoResponse>>.Success(response);
        }
        catch (Exception ex)
        {            return Result<IEnumerable<DispositivoResponse>>.Failure($"Erro ao buscar dispositivos: {ex.Message}");
        }
    }
}
