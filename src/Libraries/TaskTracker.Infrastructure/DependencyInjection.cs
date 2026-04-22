using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Application.Interfaces.Services.Common;
using TaskTracker.Infrastructure.Persistence.Repositories.Common;
using TaskTracker.Infrastructure.Services;
using TaskTracker.Infrastructure.Services.Common;

namespace TaskTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
