using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SlamCodeBlog.HidingEndpoints.Extensions;

namespace SlamCodeBlog.HidingEndpoints.Swagger
{
    public class ExcludeOnEnvironmentsDocumentsFilter : IDocumentFilter
    {
        private readonly IHostEnvironment hostEnvironment;

        public ExcludeOnEnvironmentsDocumentsFilter(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var actionDescriptor in context.ApiDescriptions)
            {
                if (actionDescriptor.CustomAttributes().OfType<ExcludeOnEnvironmentsAttribute>()
                    .Any(attr =>  attr.IsExcludedFromEnvironment(hostEnvironment.EnvironmentName)))
                {
                    swaggerDoc.Paths.Remove("/" + actionDescriptor.RelativePath); // the slash '/' is required here for match
                }
            }
        }
    }
}
