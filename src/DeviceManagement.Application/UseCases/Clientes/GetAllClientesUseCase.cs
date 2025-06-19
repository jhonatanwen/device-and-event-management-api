using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Clientes;

public sealed class GetAllClientesUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<IEnumerable<ClienteResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);

            var response = clientes.Select(c => new ClienteResponse(
                c.Id,
                c.Nome,
                c.Email,
                c.Telefone,
                c.Status));

            return Result<IEnumerable<ClienteResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ClienteResponse>>.Failure($"Erro interno: {ex.Message}");
        }
    }
}
