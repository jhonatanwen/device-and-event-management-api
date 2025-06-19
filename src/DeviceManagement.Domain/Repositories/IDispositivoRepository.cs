using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Domain.Repositories;

public interface IDispositivoRepository
{
    Task<Dispositivo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Dispositivo?> GetBySerialAsync(string serial, CancellationToken cancellationToken = default);
    Task<IEnumerable<Dispositivo>> GetByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Dispositivo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Dispositivo>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Dispositivo dispositivo, CancellationToken cancellationToken = default);
    Task UpdateAsync(Dispositivo dispositivo, CancellationToken cancellationToken = default);
    Task DeleteAsync(Dispositivo dispositivo, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySerialAsync(string serial, CancellationToken cancellationToken = default);
}
