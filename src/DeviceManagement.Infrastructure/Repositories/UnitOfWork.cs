using DeviceManagement.Domain.Repositories;
using DeviceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeviceManagement.Infrastructure.Repositories;

public sealed class UnitOfWork(DeviceManagementDbContext context) : IUnitOfWork, IDisposable
{
    private readonly DeviceManagementDbContext _context = context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

  public IClienteRepository Clientes { get; } = new ClienteRepository(context);
  public IDispositivoRepository Dispositivos { get; } = new DispositivoRepository(context);
  public IEventoRepository Eventos { get; } = new EventoRepository(context);

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            _context.Dispose();
            _disposed = true;
        }
    }
}
