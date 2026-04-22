namespace TaskTracker.Application.UnitOfWorkContracts
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        void Save();
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
