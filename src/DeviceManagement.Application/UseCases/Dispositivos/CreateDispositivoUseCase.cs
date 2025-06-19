using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dispositivos;

public sealed class CreateDispositivoUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
  public async Task<Result<Guid>> ExecuteAsync(CreateDispositivoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(request.ClienteId, cancellationToken);

            if (cliente is null)
                return Result<Guid>.Failure(ErrorMessages.NotFound.Cliente);

            if (!cliente.Status)
                return Result<Guid>.Failure(ErrorMessages.BusinessRules.ClienteInativo);

            if (await _unitOfWork.Dispositivos.ExistsBySerialAsync(request.Serial, cancellationToken))
                return Result<Guid>.Failure(ErrorMessages.BusinessRules.SerialJaExisteOutroDispositivo);

            var dispositivo = Dispositivo.Create(request.Serial, request.Imei, request.ClienteId, request.DataAtivacao);

            await _unitOfWork.Dispositivos.AddAsync(dispositivo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(dispositivo.Id);
        }
        catch (ArgumentException ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
        catch (Exception)
        {
            return Result<Guid>.Failure(ErrorMessages.Internal.CreateEntity("dispositivo"));
        }
    }
}

public sealed record CreateDispositivoRequest(
    string Serial,
    string Imei,
    Guid ClienteId,
    DateTime? DataAtivacao = null);
