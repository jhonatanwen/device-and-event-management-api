using DeviceManagement.API.DTOs;
using DeviceManagement.Application.UseCases.Dispositivos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public sealed class DispositivosController(
    CreateDispositivoUseCase createDispositivoUseCase,
    UpdateDispositivoUseCase updateDispositivoUseCase,
    GetDispositivoByIdUseCase getDispositivoByIdUseCase,
    GetDispositivosByClienteUseCase getDispositivosByClienteUseCase,
    GetAllDispositivosUseCase getAllDispositivosUseCase,
    DeleteDispositivoUseCase deleteDispositivoUseCase,
    PatchDispositivoUseCase patchDispositivoUseCase) : ControllerBase
{
    private readonly CreateDispositivoUseCase _createDispositivoUseCase = createDispositivoUseCase;
    private readonly UpdateDispositivoUseCase _updateDispositivoUseCase = updateDispositivoUseCase;
    private readonly GetDispositivoByIdUseCase _getDispositivoByIdUseCase = getDispositivoByIdUseCase;
    private readonly GetDispositivosByClienteUseCase _getDispositivosByClienteUseCase = getDispositivosByClienteUseCase;
    private readonly GetAllDispositivosUseCase _getAllDispositivosUseCase = getAllDispositivosUseCase;
    private readonly DeleteDispositivoUseCase _deleteDispositivoUseCase = deleteDispositivoUseCase;
    private readonly PatchDispositivoUseCase _patchDispositivoUseCase = patchDispositivoUseCase;

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Dispositivo>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getDispositivoByIdUseCase.ExecuteAsync(id, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        var dispositivo = new Dispositivo(
            result.Data!.Id,
            result.Data.Serial,
            result.Data.Imei,
            result.Data.DataAtivacao,
            result.Data.ClienteId);

        return Ok(dispositivo);
    }

    [HttpGet("cliente/{clienteId:guid}")]
    public async Task<ActionResult<IEnumerable<Dispositivo>>> GetByCliente(Guid clienteId, CancellationToken cancellationToken)
    {
        var result = await _getDispositivosByClienteUseCase.ExecuteAsync(clienteId, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var dispositivos = result.Data!.Select(dispositivo => new Dispositivo(
            dispositivo.Id,
            dispositivo.Serial,
            dispositivo.Imei,
            dispositivo.DataAtivacao,
            dispositivo.ClienteId
        ));

        return Ok(dispositivos);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateDispositivo dispositivo, CancellationToken cancellationToken)
    {
        var request = new CreateDispositivoRequest(dispositivo.Serial, dispositivo.Imei, dispositivo.ClienteId, dispositivo.DataAtivacao);
        var result = await _createDispositivoUseCase.ExecuteAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Dispositivo>> Update(Guid id, UpdateDispositivo dispositivo, CancellationToken cancellationToken)
    {
        var request = new UpdateDispositivoRequest(id, dispositivo.Serial, dispositivo.Imei, dispositivo.DataAtivacao);
        var result = await _updateDispositivoUseCase.ExecuteAsync(request, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

        var dispositivoAtualizado = new Dispositivo(
            result.Data!.Id,
            result.Data.Serial,
            result.Data.Imei,
            result.Data.DataAtivacao,
            result.Data.ClienteId
        );

        return Ok(dispositivoAtualizado);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<Dispositivo>> Patch(Guid id, PatchDispositivo patch, CancellationToken cancellationToken)
    {
        var request = new PatchDispositivoRequest(id, patch.DataAtivacao);
        var result = await _patchDispositivoUseCase.ExecuteAsync(request, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

        var dispositivo = new Dispositivo(
            result.Data!.Id,
            result.Data.Serial,
            result.Data.Imei,
            result.Data.DataAtivacao,
            result.Data.ClienteId
        );

        return Ok(dispositivo);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteDispositivoUseCase.ExecuteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage.Contains("não encontrado"))
                return NotFound(result.ErrorMessage);

            return BadRequest(result.ErrorMessage);
        }

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dispositivo>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllDispositivosUseCase.ExecuteAsync(cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var dispositivos = result.Data!.Select(d => new Dispositivo(d.Id, d.Serial, d.Imei, d.DataAtivacao, d.ClienteId));
        return Ok(dispositivos);
    }
}
