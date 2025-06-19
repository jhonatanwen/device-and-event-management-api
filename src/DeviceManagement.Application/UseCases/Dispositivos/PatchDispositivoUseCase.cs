using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dispositivos;

public sealed class PatchDispositivoUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<DispositivoResponse>> ExecuteAsync(PatchDispositivoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var dispositivo = await _unitOfWork.Dispositivos.GetByIdAsync(request.Id, cancellationToken);

            if (dispositivo is null)
                return Result<DispositivoResponse>.Failure("Dispositivo não encontrado");
            if (request.DataAtivacao.HasValue)
            {
                if (request.DataAtivacao.Value != default)
                {
                    dispositivo.Ativar();
                }
                else
                {
                    dispositivo.Desativar();
                }
            }

            await _unitOfWork.Dispositivos.UpdateAsync(dispositivo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new DispositivoResponse(
                dispositivo.Id,
                dispositivo.Serial,
                dispositivo.Imei,
                dispositivo.DataAtivacao,
                dispositivo.ClienteId);

            return Result<DispositivoResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<DispositivoResponse>.Failure($"Erro ao atualizar dispositivo: {ex.Message}");
        }
    }
}

public sealed record PatchDispositivoRequest(
    Guid Id,
    DateTime? DataAtivacao = null);
