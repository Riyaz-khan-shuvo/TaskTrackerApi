using Autofac;
using TaskTracker.Application.UnitOfWorkContracts;
using TaskTracker.Infrastructure.Persistence.UnitOfWork;

namespace TaskTracker.API.DependencyInjection
{
    public class UnitOfWorkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TaskTrackerUnitOfWork>()
                   .As<ITaskTrackerUnitOfWork>()
                   .InstancePerLifetimeScope();
        }
    }
}
