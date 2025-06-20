using DeviceManagement.API.Contracts;
using DeviceManagement.Application.UseCases.Eventos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public sealed class EventosController(
    CreateEventoUseCase createEventoUseCase,
    GetEventosByPeriodUseCase getEventosByPeriodUseCase,
    GetEventosByDispositivoUseCase getEventosByDispositivoUseCase,
    GetAllEventosUseCase getAllEventosUseCase,
    GetEventoByIdUseCase getEventoByIdUseCase,
    DeleteEventoUseCase deleteEventoUseCase) : ControllerBase
{
    private readonly CreateEventoUseCase _createEventoUseCase = createEventoUseCase;
    private readonly GetEventosByPeriodUseCase _getEventosByPeriodUseCase = getEventosByPeriodUseCase;
    private readonly GetEventosByDispositivoUseCase _getEventosByDispositivoUseCase = getEventosByDispositivoUseCase;
    private readonly GetAllEventosUseCase _getAllEventosUseCase = getAllEventosUseCase;
    private readonly GetEventoByIdUseCase _getEventoByIdUseCase = getEventoByIdUseCase;
    private readonly DeleteEventoUseCase _deleteEventoUseCase = deleteEventoUseCase;

  [HttpGet]
    public async Task<ActionResult<IEnumerable<Evento>>> GetByPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        var result = await _getEventosByPeriodUseCase.ExecuteAsync(startDate, endDate, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var eventos = result.Data!.Select(evento => new Evento(
            evento.Id, evento.Tipo, evento.DataHora, evento.DispositivoId
        ));
        return Ok(eventos);
    }

    [HttpGet("dispositivo/{dispositivoId:guid}")]
    public async Task<ActionResult<IEnumerable<Evento>>> GetByDispositivo(
        Guid dispositivoId,
        CancellationToken cancellationToken)
    {
        var result = await _getEventosByDispositivoUseCase.ExecuteAsync(dispositivoId, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var eventos = result.Data!.Select(e => new Evento(e.Id, e.Tipo, e.DataHora, e.DispositivoId));
        return Ok(eventos);
    }    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateEvento evento, CancellationToken cancellationToken)
    {
        var request = new CreateEventoRequest(evento.Tipo, evento.DispositivoId, evento.DataHora);
        var result = await _createEventoUseCase.ExecuteAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Evento>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllEventosUseCase.ExecuteAsync(cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var eventos = result.Data!.Select(e => new Evento(e.Id, e.Tipo, e.DataHora, e.DispositivoId));
        return Ok(eventos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Evento>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getEventoByIdUseCase.ExecuteAsync(id, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        var evento = new Evento(
            result.Data!.Id,
            result.Data.Tipo,
            result.Data.DataHora,
            result.Data.DispositivoId);

        return Ok(evento);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteEventoUseCase.ExecuteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage.Contains("não encontrado"))
                return NotFound(result.ErrorMessage);

            return BadRequest(result.ErrorMessage);
        }

        return NoContent();
    }
}
