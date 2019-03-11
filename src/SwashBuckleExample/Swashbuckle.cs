using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwashBuckleExample
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                // Remove the version parameter as it's in the url
                options.OperationFilter<RemoveVersionFromParameter>();

                // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                options.SwaggerDoc(
                    description.GroupName,
                    new Info()
                    {
                        Title = $"My API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "An API like no other.",
                        Contact = new Contact()
                        {
                            Name = "Mister Api",
                            Url = "https://github.com/yetanotherchris"
                        }
                    } );
            }

            // Add XML docs into Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

        public class RemoveVersionFromParameter : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                operation.Parameters.Remove(versionParameter);
            }
        }

        public class ReplaceVersionWithExactValueInPath : IDocumentFilter
        {
            public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Paths = swaggerDoc.Paths
                    .ToDictionary(
                        path => path.Key.Replace("v{version}", $"v{swaggerDoc.Info.Version}"),
                        path => path.Value
                    );
            }
        }
    }
}