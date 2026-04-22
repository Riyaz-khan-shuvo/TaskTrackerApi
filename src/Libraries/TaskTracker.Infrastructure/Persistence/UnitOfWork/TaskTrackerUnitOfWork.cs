using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.UnitOfWorkContracts;
using TaskTracker.Infrastructure.Persistence.DataContext;

namespace TaskTracker.Infrastructure.Persistence.UnitOfWork
{
    public class TaskTrackerUnitOfWork : UnitOfWork, ITaskTrackerUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        public TaskTrackerUnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
            : base(context)
        {
            _serviceProvider = serviceProvider;
        }

        public IMenuAuthorizationRepository MenuAuthorizationRepository => Repository<IMenuAuthorizationRepository>();
        public IUserMenuRepository UserMenuRepository => Repository<IUserMenuRepository>();

        public ITaskRepository TaskRepository => Repository<ITaskRepository>();

        // Generic dynamic resolution for less-used repositories
        public TRepository Repository<TRepository>() where TRepository : class
        {
            return _serviceProvider.GetRequiredService<TRepository>();
        }
    }
}