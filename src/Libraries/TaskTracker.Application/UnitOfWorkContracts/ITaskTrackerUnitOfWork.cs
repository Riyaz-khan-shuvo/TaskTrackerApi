using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.UnitOfWorkContracts
{
    public interface ITaskTrackerUnitOfWork : IUnitOfWork
    {


        IMenuAuthorizationRepository MenuAuthorizationRepository { get; }
        IUserMenuRepository UserMenuRepository { get; }
        ITaskRepository TaskRepository { get; }

        //ITestRepository TestRepository { get; }
        TRepository Repository<TRepository>() where TRepository : class;
    }
}
