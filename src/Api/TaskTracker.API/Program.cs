using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Text.Json.Serialization;
using TaskTracker.API;
using TaskTracker.API.DependencyInjection;
using TaskTracker.API.swagger;
using TaskTracker.Application;
using TaskTracker.Infrastructure;
using TaskTracker.Infrastructure.Extensions;
using TaskTracker.Infrastructure.Middlewares;
using TaskTracker.Shared.Middlewares;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Bootstrap Serilog
Log.Logger = SerilogConfiguration.CreateBootstrapLogger(configuration);

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog configuration
    builder.Host.UseSerilog((ctx, lc) => SerilogConfiguration.ConfigureLogger(ctx, lc));

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<RepositoryModule>();
        containerBuilder.RegisterModule<ServiceModule>();
        containerBuilder.RegisterModule<UnitOfWorkModule>();
    });

    // Add services
    builder.Services.AddDbContexts(builder.Configuration);
    builder.Services.AddIdentity();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddCorsPolicy(builder.Configuration);
    builder.Services.AddApplicationService(builder.Configuration);
    builder.Services.ConfigureSwagger();
    builder.Services.AddInfrastructureService(builder.Configuration);
    builder.Services.AddMemoryCache();

    //builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    //.AddEntityFrameworkStores<ApplicationDbContext>()
    //.AddDefaultTokenProviders();

    builder.Services.AddControllers(config =>
    {
        var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
        config.Filters.Add(new AuthorizeFilter(policy));
    }).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;

    builder.Services.AddSwaggerGen();
    builder.Services.AddOpenApiDocument();

    var app = builder.Build();
    app.UseCustomExceptionHandler(); // global exception handler
    app.UseStaticFiles(); // Enables serving static files from wwwroot

    // Middleware pipeline
    app.UseHttpsRedirection();
    app.UseCors("AllowFrontend");       // Must come before auth
                                        // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<MenuAuthorizationMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerDocumentation(app.Environment);
    }


    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "TaskTracker Application start-up failed");

}
finally
{
    Log.CloseAndFlush();
}