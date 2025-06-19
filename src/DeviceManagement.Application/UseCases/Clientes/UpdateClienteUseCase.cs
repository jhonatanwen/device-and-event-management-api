using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Clientes;

public sealed class UpdateClienteUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result> ExecuteAsync(Guid id, UpdateClienteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);

            if (cliente is null)
                return Result.Failure("Cliente não encontrado");

            if (cliente.Email != request.Email && await _unitOfWork.Clientes.ExistsByEmailAsync(request.Email, cancellationToken))
                return Result.Failure("Email já está em uso por outro cliente");

            cliente.UpdateNome(request.Nome);
            cliente.UpdateEmail(request.Email);
            cliente.UpdateTelefone(request.Telefone);

            await _unitOfWork.Clientes.UpdateAsync(cliente, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro interno: {ex.Message}");
        }
    }
}

public sealed record UpdateClienteRequest(
    string Nome,
    string Email,
    string? Telefone);
