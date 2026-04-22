using Microsoft.OpenApi.Models;

namespace TaskTracker.API.swagger
{
    public static class ConfigureSwaggerService
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(
                c =>
                {
                    c.UseInlineDefinitionsForEnums();
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    c.AddSecurityDefinition(
                        "Bearer",
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "JWT Authorization header using the Bearer scheme."
                        });

                    c.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                        });
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "TaskTracker API",
                            Description = "An ASP.NET Core Web API for TaskTracker ",
                            Contact = new OpenApiContact { Name = "Riyaz Hossain", Email = "mdriyaz5965@gmail.com" },
                            License =
                                new OpenApiLicense
                                {
                                    Name = "Apache License",
                                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.txt")
                                }
                        });
                });
        }

        public static void UseSwaggerDocumentation(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(env);

            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
                    c.InjectStylesheet("/swagger/theme-feeling-blue.css");
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
            app.UseReDoc(
                options =>
                {
                    options.DocumentTitle = "API Documentation";
                    options.SpecUrl = "/swagger/v1/swagger.json";
                    options.RoutePrefix = "api-docs";
                });
        }
    }
}
