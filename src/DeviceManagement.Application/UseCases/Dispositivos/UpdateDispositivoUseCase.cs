using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dispositivos;

public sealed class UpdateDispositivoUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Dispositivo>> ExecuteAsync(UpdateDispositivoRequest request, CancellationToken cancellationToken)
    {
        var dispositivo = await _unitOfWork.Dispositivos.GetByIdAsync(request.Id, cancellationToken);
        if (dispositivo is null)
            return Result<Dispositivo>.Failure("Dispositivo não encontrado");

        try
        {
            // Atualizar serial se fornecido
            if (!string.IsNullOrWhiteSpace(request.Serial))
                dispositivo.UpdateSerial(request.Serial);

            // Atualizar IMEI se fornecido
            if (!string.IsNullOrWhiteSpace(request.Imei))
                dispositivo.UpdateImei(request.Imei);

            // Gerenciar ativação/desativação
            if (request.DataAtivacao.HasValue && !dispositivo.EstaAtivo)
            {
                dispositivo.Ativar();
            }
            else if (!request.DataAtivacao.HasValue && dispositivo.EstaAtivo)
            {
                dispositivo.Desativar();
            }

            await _unitOfWork.Dispositivos.UpdateAsync(dispositivo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Dispositivo>.Success(dispositivo);
        }
        catch (ArgumentException ex)
        {
            return Result<Dispositivo>.Failure(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Dispositivo>.Failure(ex.Message);
        }
    }
}

public sealed record UpdateDispositivoRequest(
    Guid Id,
    string? Serial,
    string? Imei,
    DateTime? DataAtivacao);
