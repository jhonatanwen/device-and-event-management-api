using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Domain.Repositories;

public interface IEventoRepository
{
    Task<Evento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Evento>> GetByDispositivoIdAsync(Guid dispositivoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Evento>> GetByPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Evento>> GetByDispositivoAndPeriodAsync(Guid dispositivoId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<Dictionary<EventType, int>> GetEventCountByTypeForPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Evento>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Evento evento, CancellationToken cancellationToken = default);
    Task UpdateAsync(Evento evento, CancellationToken cancellationToken = default);
    Task DeleteAsync(Evento evento, CancellationToken cancellationToken = default);
}
