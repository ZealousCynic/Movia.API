using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;

namespace WebApiNinjectStudio
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _Provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._Provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in this._Provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
            // Uses full schema names to avoid v1/v2/v3 schema collisions
            options.CustomSchemaIds((type) => type.FullName);

        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Movia RRE API",
                Version = description.ApiVersion.ToString(),
                Description = "Movia RRE application with Swagger, Swashbuckle, and API versioning.",
                Contact = new OpenApiContact() { Name = "Movia", Email = "Service@movia.dk" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
