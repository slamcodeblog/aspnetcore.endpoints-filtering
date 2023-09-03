using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SlamCodeBlog.HidingEndpoints.Extensions;

namespace SlamCodeBlog.HidingEndpoints.Swagger
{
    public static class SwaggerConfigurationExtensions
    {
        public static bool IsIncludedInEnvironment(string docId, ApiDescription apiDescription, IHostEnvironment hostEnvironment)
        {
            // search for any mathcing excluded envirnment attribute declaration
            if (apiDescription.ActionDescriptor.EndpointMetadata.OfType<ExcludeOnEnvironmentsAttribute>()
                .Any(attr => attr.Environments.Contains(hostEnvironment.EnvironmentName)))
            {
                return false;
            }

            return true;
        }
    }
}
