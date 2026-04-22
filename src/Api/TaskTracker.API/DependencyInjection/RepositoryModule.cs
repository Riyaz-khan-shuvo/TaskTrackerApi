using Autofac;
using System.Reflection;
using TaskTracker.Infrastructure.Persistence.Repositories;

namespace TaskTracker.API.DependencyInjection
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetAssembly(typeof(MenuAuthorizationRepository));

            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Repository"))  
                   .AsImplementedInterfaces()                 
                   .InstancePerLifetimeScope();               
        }
    }
}
