using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Infrastructure.Persistence.Repositories.Common
{
    public abstract class BaseRepository
    {
        protected readonly IMapper _mapper;

        protected BaseRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected TEntity MapToEntity<TDto, TEntity>(TDto dto) => _mapper.Map<TEntity>(dto);

        protected TDto MapToDto<TDto, TEntity>(TEntity entity) => _mapper.Map<TDto>(entity);

        protected async Task<TDto> InsertAsync<TDto, TEntity>(DbSet<TEntity> dbSet, TDto dto, Action<TEntity>? beforeSave = null, DbContext? context = null)
            where TEntity : class
        {
            var entity = MapToEntity<TDto, TEntity>(dto);
            beforeSave?.Invoke(entity);
            dbSet.Add(entity);
            if (context != null)
                await context.SaveChangesAsync();
            return MapToDto<TDto, TEntity>(entity);
        }

        protected async Task<TDto> UpdateAsync<TDto, TEntity>(DbSet<TEntity> dbSet, TDto dto, Action<TEntity>? beforeSave = null, DbContext? context = null)
            where TEntity : class
        {
            var entity = MapToEntity<TDto, TEntity>(dto);
            beforeSave?.Invoke(entity);
            dbSet.Update(entity);
            if (context != null)
                await context.SaveChangesAsync();
            return MapToDto<TDto, TEntity>(entity);
        }
    }


}
