using TaskTracker.Application.DTOs.Common;
using TaskTracker.Core.Common;

namespace TaskTracker.Application.Interfaces.Repositories.Common
{
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : class, IEntityBase<TKey>, new()
    {
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        TEntity Insert(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        TEntity Update(TEntity entity);

        Task<ResultVM> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        Task<ResultVM> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
        ResultVM Delete(TKey id);

        Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        TEntity GetById(TKey id);

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetAll();
    }
}
