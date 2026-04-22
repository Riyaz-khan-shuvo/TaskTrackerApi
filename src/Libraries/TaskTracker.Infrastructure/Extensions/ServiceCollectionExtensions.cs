using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Core.Entities.Auth;
using TaskTracker.Infrastructure.Persistence.DataContext;

namespace TaskTracker.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Add DbContexts
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var TaskTrackerConnection = configuration.GetConnectionString("TaskTrackerConnection");
            var TaskTrackerAuthConnection = configuration.GetConnectionString("TaskTrackerAuthConnection");

            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(TaskTrackerConnection));

            services.AddDbContextPool<AuthDbContext>(options =>
                options.UseSqlServer(TaskTrackerAuthConnection));

            return services;
        }

        // Configure Identity
        public static void AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            // Authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomAccess", policy =>
                {
                    policy.RequireRole("Member");
                    policy.RequireRole("Support");
                });

                options.AddPolicy("CreatePermission", policy =>
                {
                    policy.RequireClaim("create", "true");
                });
            });
        }

        // Optional: Cookie authentication for admin portal
        public static void AddCookieAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.Cookie.Name = "TaskTracker.Identity";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });
        }

        // CORS policy for frontend apps



        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var allowFrontend = configuration.GetSection("Cors:AllowFrontend").Get<string[]>();
            var allowHeaders = configuration.GetSection("Cors:AllowHeaders").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(allowFrontend)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });

                options.AddPolicy("AllowHeaders", policy =>
                {
                    policy.WithOrigins(allowHeaders)
                          .WithHeaders(
                              "Content-Type",
                              "Server",
                              "Access-Control-Allow-Headers",
                              "Access-Control-Expose-Headers",
                              "x-custom-header",
                              "x-path",
                              "x-record-in-use",
                              "Content-Disposition"
                          );
                });
            });

            return services;
        }
    }
}





























//using System.Text;
//using TaskTracker.Core.Entities.Auth;
//using TaskTracker.Core.Entities.Auth.Requirements;
//using TaskTracker.Infrastructure.Persistence.DataContext;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;

//namespace TaskTracker.Infrastructure.Extensions
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static IServiceCollection AddTaskTrackerDbContexts(this IServiceCollection services, IConfiguration configuration)
//        {
//            var TaskTrackerConnection = configuration.GetConnectionString("TaskTrackerConnection");
//            var TaskTrackerAuthConnection = configuration.GetConnectionString("TaskTrackerAuthConnection");

//            services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(TaskTrackerConnection,
//                    x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

//            services.AddDbContext<AuthDbContext>(options =>
//                options.UseSqlServer(TaskTrackerAuthConnection,
//                    x => x.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

//            return services;
//        }


//        public static void AddIdentity(this IServiceCollection services)
//        {
//            services
//                .AddIdentity<ApplicationUser, ApplicationRole>()
//                .AddEntityFrameworkStores<AuthDbContext>()
//                .AddUserManager<ApplicationUserManager>()
//                .AddRoleManager<ApplicationRoleManager>()
//                .AddSignInManager<ApplicationSignInManager>()
//                .AddDefaultTokenProviders();

//            services.Configure<IdentityOptions>(options =>
//            {
//                // Password settings.
//                options.Password.RequireDigit = true;
//                options.Password.RequireLowercase = false;
//                options.Password.RequireNonAlphanumeric = false;
//                options.Password.RequireUppercase = false;
//                options.Password.RequiredLength = 6;
//                options.Password.RequiredUniqueChars = 0;

//                // Lockout settings.
//                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//                options.Lockout.MaxFailedAccessAttempts = 5;
//                options.Lockout.AllowedForNewUsers = true;

//                // User settings.
//                options.User.AllowedUserNameCharacters =
//                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//                options.User.RequireUniqueEmail = true;
//            });

//            services.AddAuthorization(options =>
//            {
//                options.AddPolicy("CustomAccess", policy =>
//                {
//                    policy.RequireRole("Member");
//                    policy.RequireRole("Support");
//                });

//                options.AddPolicy("CreatePermission", policy =>
//                {
//                    policy.RequireClaim("create", "true");
//                });

//                options.AddPolicy("AgeRestriction", policy =>
//                {
//                    policy.Requirements.Add(new AgeRequirement());
//                });
//            });

//            services.AddSingleton<IAuthorizationHandler, AgeRequirementHandler>();
//        }

//        public static void AddCookieAuthentication(this IServiceCollection services)
//        {
//            services.AddAuthentication()
//                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//                {
//                    options.LoginPath = new PathString("/Account/Login");
//                    options.AccessDeniedPath = new PathString("/Account/Login");
//                    options.LogoutPath = new PathString("/Account/Logout");
//                    options.Cookie.Name = "FirstDemoPortal.Identity";
//                    options.SlidingExpiration = true;
//                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
//                });
//        }

//        public static void AddJwtAuthentication(this IServiceCollection services,
//            string key, string issuer, string audience)
//        {
//            services.AddAuthentication()
//                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
//                {
//                    x.RequireHttpsMetadata = false;
//                    x.SaveToken = true;
//                    x.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuerSigningKey = true,
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
//                        ValidateIssuer = true,
//                        ValidateAudience = true,
//                        ValidIssuer = issuer,
//                        ValidAudience = audience,
//                    };
//                });

//            services.AddAuthorization(options =>
//            {
//                options.AddPolicy("AgeRestriction", policy =>
//                {
//                    policy.AuthenticationSchemes.Clear();
//                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
//                    policy.RequireAuthenticatedUser();
//                    policy.Requirements.Add(new AgeRequirement());
//                });
//            });
//        }


//    }
//}
