namespace DeviceManagement.Domain.Repositories;

public interface IUnitOfWork
{
    IClienteRepository Clientes { get; }
    IDispositivoRepository Dispositivos { get; }
    IEventoRepository Eventos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
