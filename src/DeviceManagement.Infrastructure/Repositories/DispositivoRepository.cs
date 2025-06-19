using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Repositories;

public sealed class DispositivoRepository(DeviceManagementDbContext context) : IDispositivoRepository
{
    private readonly DeviceManagementDbContext _context = context;

  public async Task<Dispositivo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Dispositivos
            .Include(dispositivo => dispositivo.Cliente)
            .Include(dispositivo => dispositivo.Eventos)
            .FirstOrDefaultAsync(dispositivo => dispositivo.Id == id, cancellationToken);
    }

    public async Task<Dispositivo?> GetBySerialAsync(string serial, CancellationToken cancellationToken = default)
    {
        return await _context.Dispositivos
            .Include(dispositivo => dispositivo.Cliente)
            .Include(dispositivo => dispositivo.Eventos)
            .FirstOrDefaultAsync(dispositivo => dispositivo.Serial == serial, cancellationToken);
    }

    public async Task<IEnumerable<Dispositivo>> GetByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken = default)
    {
        return await _context.Dispositivos
            .Include(dispositivo => dispositivo.Cliente)
            .Include(dispositivo => dispositivo.Eventos)
            .Where(dispositivo => dispositivo.ClienteId == clienteId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Dispositivo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Dispositivos
            .Include(dispositivo => dispositivo.Cliente)
            .Include(dispositivo => dispositivo.Eventos)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Dispositivo>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Dispositivos
            .Include(dispositivo => dispositivo.Cliente)
            .Include(dispositivo => dispositivo.Eventos)
            .Where(dispositivo => dispositivo.DataAtivacao.HasValue)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Dispositivo dispositivo, CancellationToken cancellationToken = default)
    {
        await _context.Dispositivos.AddAsync(dispositivo, cancellationToken);
    }

    public Task UpdateAsync(Dispositivo dispositivo, CancellationToken cancellationToken = default)
    {
        _context.Dispositivos.Update(dispositivo);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Dispositivo dispositivo, CancellationToken cancellationToken = default)
    {
        _context.Dispositivos.Remove(dispositivo);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsBySerialAsync(string serial, CancellationToken cancellationToken = default)
    {
        return await _context.Dispositivos
            .AnyAsync(dispositivo => dispositivo.Serial == serial, cancellationToken);
    }
}
