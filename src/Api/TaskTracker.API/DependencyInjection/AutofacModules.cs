using Autofac;

namespace TaskTracker.API.DependencyInjection
{
    public static class AutofacModules
    {
        public static void Configure(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<RepositoryModule>();
            containerBuilder.RegisterModule<ServiceModule>();
            containerBuilder.RegisterModule<UnitOfWorkModule>();
        }
    }
}
