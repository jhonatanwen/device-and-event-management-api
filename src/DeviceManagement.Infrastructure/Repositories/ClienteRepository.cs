using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Repositories;

public sealed class ClienteRepository(DeviceManagementDbContext context) : IClienteRepository
{
    private readonly DeviceManagementDbContext _context = context;

  public async Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(cliente => cliente.Dispositivos)
            .FirstOrDefaultAsync(cliente => cliente.Id == id, cancellationToken);
    }

    public async Task<Cliente?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(cliente => cliente.Dispositivos)
            .FirstOrDefaultAsync(cliente => cliente.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(cliente => cliente.Dispositivos)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Cliente>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(cliente => cliente.Dispositivos)
            .Where(cliente => cliente.Status)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
    }

    public Task UpdateAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _context.Clientes.Remove(cliente);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AnyAsync(cliente => cliente.Email == email, cancellationToken);
    }
}
