using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SlamCodeBlog.HidingEndpoints.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection.Metadata;

namespace SlamCodeBlog.HidingEndpoints.Swagger
{
    public class ApiSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly IApiVersionDescriptionProvider _provider;

        public ApiSwaggerOptions(
            IHostEnvironment hostEnvironment,
            IApiVersionDescriptionProvider apiVersionProvider)
        {
            this.hostEnvironment = hostEnvironment;
            _provider = apiVersionProvider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.DocInclusionPredicate(IncludeDocument);
            // add swagger document for every API version discovered
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

        private bool IncludeDocument(string docId, ApiDescription apiDescription)
        {  
            // check only endpoints belonging to this document
            if (docId != apiDescription.GroupName)
                return false;

            if (apiDescription.ActionDescriptor.EndpointMetadata.OfType<ExcludeOnEnvironmentsAttribute>()
                .Any(attr => attr.Environments.Contains(hostEnvironment.EnvironmentName)))
            {
                return false;
            }

            return true;
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(
                ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = "Service API",
                Version = desc.ApiVersion.ToString()
            };

            return info;
        }
    }
}
