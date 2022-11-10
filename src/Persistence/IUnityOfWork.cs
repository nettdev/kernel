namespace Nett.SharedKernel;

public interface IUnitOfWork : IDisposable
{
    Task<bool> Commit(CancellationToken cancellationToken = default(CancellationToken));
}