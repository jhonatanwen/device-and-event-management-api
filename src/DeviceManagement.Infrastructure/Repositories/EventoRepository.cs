using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Repositories;

public sealed class EventoRepository(DeviceManagementDbContext context) : IEventoRepository
{
    private readonly DeviceManagementDbContext _context = context;

  public async Task<Evento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Eventos
            .Include(evento => evento.Dispositivo)
            .FirstOrDefaultAsync(evento => evento.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Evento>> GetByDispositivoIdAsync(Guid dispositivoId, CancellationToken cancellationToken = default)
    {
        return await _context.Eventos
            .Include(evento => evento.Dispositivo)
            .Where(evento => evento.DispositivoId == dispositivoId)
            .OrderByDescending(evento => evento.DataHora)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Evento>> GetByPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Eventos
            .Include(evento => evento.Dispositivo)
            .Where(evento => evento.DataHora >= startDate && evento.DataHora <= endDate)
            .OrderByDescending(evento => evento.DataHora)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Evento>> GetByDispositivoAndPeriodAsync(Guid dispositivoId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Eventos
            .Include(evento => evento.Dispositivo)
            .Where(evento => evento.DispositivoId == dispositivoId && evento.DataHora >= startDate && evento.DataHora <= endDate)
            .OrderByDescending(evento => evento.DataHora)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<EventType, int>> GetEventCountByTypeForPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Eventos
            .Where(evento => evento.DataHora >= startDate && evento.DataHora <= endDate)
            .GroupBy(evento => evento.Tipo)
            .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);
    }

    public async Task<IEnumerable<Evento>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Eventos
            .Include(evento => evento.Dispositivo)
            .OrderByDescending(evento => evento.DataHora)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Evento evento, CancellationToken cancellationToken = default)
    {
        await _context.Eventos.AddAsync(evento, cancellationToken);
    }

    public Task UpdateAsync(Evento evento, CancellationToken cancellationToken = default)
    {
        _context.Eventos.Update(evento);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Evento evento, CancellationToken cancellationToken = default)
    {
        _context.Eventos.Remove(evento);
        return Task.CompletedTask;
    }
}
