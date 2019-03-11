using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwashBuckleExample
{
    public static class Extensions
    {
        public static IServiceCollection AddMvcAndVersionedSwagger(this IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddDataAnnotations()
                .AddApiExplorer()
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            services
                .AddSwaggerGen()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddApiVersioning(options => options.AssumeDefaultVersionWhenUnspecified = true);

            return services;
        }

        public static IApplicationBuilder AddVersionedSwaggerWithUI(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant() );
                    }
                    options.RoutePrefix = string.Empty;
                });
        }

        public static IApplicationBuilder UseJsonExceptionHandler(this IApplicationBuilder app, IHostingEnvironment environment)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = new JsonExceptionMiddleware(environment).Invoke
            });

            return app;
        }
    }
}