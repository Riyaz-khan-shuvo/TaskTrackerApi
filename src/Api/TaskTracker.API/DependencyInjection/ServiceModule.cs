using Autofac;
using TaskTracker.Application.Interfaces.Services.Common;
using TaskTracker.Infrastructure.Services.Common;

namespace TaskTracker.API.DependencyInjection
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GridService<>))
                   .As(typeof(IGridService<>))
                   .InstancePerLifetimeScope();
        }
    }
}
