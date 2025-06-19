using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dispositivos;

public sealed class GetDispositivosByClienteUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDispositivosByClienteUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<DispositivoResponse>>> ExecuteAsync(Guid clienteId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId, cancellationToken);

            if (cliente is null)
                return Result<IEnumerable<DispositivoResponse>>.Failure("Cliente não encontrado");

            var dispositivos = await _unitOfWork.Dispositivos.GetByClienteIdAsync(clienteId, cancellationToken);

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
        {
            return Result<IEnumerable<DispositivoResponse>>.Failure($"Erro interno: {ex.Message}");
        }
    }
}
