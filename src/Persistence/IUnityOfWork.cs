namespace Nett.Kernel;

public interface IUnitOfWork : IDisposable
{
    Task<bool> Commit(CancellationToken cancellationToken = default(CancellationToken));
}