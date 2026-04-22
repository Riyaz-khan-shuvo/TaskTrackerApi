using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Core.Common;
using TaskTracker.Infrastructure.Persistence.DataContext;

namespace TaskTracker.Infrastructure.Persistence.Repositories.Common
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class, IEntityBase<TKey>, new()
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public TEntity Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public TEntity Update(TEntity entity)
        {
            _dbSet.Update(entity);
            return entity;
        }


        public async Task<ResultVM> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            var entities = await _dbSet
                .Where(e => ids.Contains(EF.Property<TKey>(e, "Id")))
                .ToListAsync(cancellationToken);

            if (!entities.Any())
                return ResultVM.Fail("No matching data found");

            foreach (var entity in entities)
            {
                if (entity is ISoftDeletable softEntity)
                {
                    softEntity.IsActive = false;
                    softEntity.IsArchive = true;
                    _dbSet.Update(entity);
                }
                //else
                //{
                //    _dbSet.Remove(entity);
                //}
            }

            return ResultVM.Success($"{entities.Count} record(s) deleted successfully");
        }



        public async Task<ResultVM> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity == null)
                return ResultVM.Fail("Data not found");

            if (entity is ISoftDeletable softEntity)
            {
                softEntity.IsActive = false;
                softEntity.IsArchive = true;

                _dbSet.Update(entity);
                return ResultVM.Success("Data deleted successfully");
            }
            return ResultVM.Fail("Error delete Data");
            //else
            //{
            //    _dbSet.Remove(entity);
            //    return ResultVM.Success("Data deleted successfully");
            //}
        }

        public ResultVM Delete(TKey id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
                return ResultVM.Fail("Data not found");

            if (entity is ISoftDeletable softEntity)
            {
                softEntity.IsActive = false;
                softEntity.IsArchive = true;
                //softEntity.LastModifiedOn = DateTime.UtcNow;

                _dbSet.Update(entity);
                return ResultVM.Success("Data deleted successfully");
            }
            else
            {
                _dbSet.Remove(entity);
                return ResultVM.Success("Data deleted successfully");
            }
        }

        public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public TEntity GetById(TKey id)
        {
            return _dbSet.Find(id);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                return await _dbSet.AsNoTracking()
                                   .Where(e => ((ISoftDeletable)e).IsActive && !((ISoftDeletable)e).IsArchive)
                                   .ToListAsync(cancellationToken);
            }

            return await _dbSet.AsNoTracking()
                               .ToListAsync(cancellationToken);
        }

        public IEnumerable<TEntity> GetAll()
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                return _dbSet.AsNoTracking()
                             .Where(e => ((ISoftDeletable)e).IsActive && !((ISoftDeletable)e).IsArchive);
            }

            return _dbSet.AsNoTracking();
        }

    }
}
