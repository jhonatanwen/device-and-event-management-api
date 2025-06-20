using DeviceManagement.API.Contracts;
using DeviceManagement.Application.UseCases.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public sealed class ClientesController(
    CreateClienteUseCase createClienteUseCase,
    GetClienteByIdUseCase getClienteByIdUseCase,
    GetAllClientesUseCase getAllClientesUseCase,
    UpdateClienteUseCase updateClienteUseCase,
    DeleteClienteUseCase deleteClienteUseCase,
    PatchClienteUseCase patchClienteUseCase) : ControllerBase
{    private readonly CreateClienteUseCase _createClienteUseCase = createClienteUseCase;
    private readonly GetClienteByIdUseCase _getClienteByIdUseCase = getClienteByIdUseCase;
    private readonly GetAllClientesUseCase _getAllClientesUseCase = getAllClientesUseCase;
    private readonly UpdateClienteUseCase _updateClienteUseCase = updateClienteUseCase;
    private readonly DeleteClienteUseCase _deleteClienteUseCase = deleteClienteUseCase;
    private readonly PatchClienteUseCase _patchClienteUseCase = patchClienteUseCase;

  [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllClientesUseCase.ExecuteAsync(cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var clientes = result.Data!.Select(cliente => new Cliente(
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.Telefone,
            cliente.Status
        ));

        return Ok(clientes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Cliente>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getClienteByIdUseCase.ExecuteAsync(id, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        var cliente = new Cliente(
            result.Data!.Id,
            result.Data.Nome,
            result.Data.Email,
            result.Data.Telefone,
            result.Data.Status
        );

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateCliente cliente, CancellationToken cancellationToken)
    {
        var request = new CreateClienteRequest(cliente.Nome, cliente.Email, cliente.Telefone, cliente.Status);
        var result = await _createClienteUseCase.ExecuteAsync(request, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateCliente cliente, CancellationToken cancellationToken)
    {
        var request = new UpdateClienteRequest(cliente.Nome, cliente.Email, cliente.Telefone);
        var result = await _updateClienteUseCase.ExecuteAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage.Contains("não encontrado"))
                return NotFound(result.ErrorMessage);

            return BadRequest(result.ErrorMessage);
        }

        var clienteAtualizado = new Cliente(
            result.Data!.Id,
            result.Data.Nome,
            result.Data.Email,
            result.Data.Telefone,
            result.Data.Status
        );

        return Ok(clienteAtualizado);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteClienteUseCase.ExecuteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage.Contains("não encontrado"))
                return NotFound(result.ErrorMessage);

            return BadRequest(result.ErrorMessage);
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<Cliente>> Patch(Guid id, PatchCliente patch, CancellationToken cancellationToken)
    {
        var request = new PatchClienteRequest(id, patch.Status);
        var result = await _patchClienteUseCase.ExecuteAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage.Contains("não encontrado"))
                return NotFound(result.ErrorMessage);

            return BadRequest(result.ErrorMessage);
        }

        var clienteAtualizado = new Cliente(
            result.Data!.Id,
            result.Data.Nome,
            result.Data.Email,
            result.Data.Telefone,
            result.Data.Status);

        return Ok(clienteAtualizado);
    }
}
