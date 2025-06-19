using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Clientes;

public sealed class CreateClienteUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Guid>> ExecuteAsync(CreateClienteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _unitOfWork.Clientes.ExistsByEmailAsync(request.Email, cancellationToken))
                return Result<Guid>.Failure("Email já está em uso por outro cliente");

            var cliente = Cliente.Create(request.Nome, request.Email, request.Telefone, request.Status);

            await _unitOfWork.Clientes.AddAsync(cliente, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(cliente.Id);
        }
        catch (ArgumentException ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Erro interno: {ex.Message}");
        }
    }
}

public sealed record CreateClienteRequest(
    string Nome,
    string Email,
    string? Telefone = null,
    bool Status = true);
