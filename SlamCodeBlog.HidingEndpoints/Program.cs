using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using SlamCodeBlog.HidingEndpoints.Extensions;
using SlamCodeBlog.HidingEndpoints.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using static SlamCodeBlog.HidingEndpoints.Swagger.SwaggerConfigurationExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddEndpointsApiExplorer();

// swagger cfg
builder.Services.AddSwaggerGen()
    .ConfigureOptions<ApiSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg => {
        var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>(); // swagger provider service resolved

        foreach (var groupName in apiVersionProvider.ApiVersionDescriptions.Select(versionInfo => versionInfo.GroupName))
        {
            if (swaggerProvider.GetSwagger(groupName).Paths.Any()) // this check added
            {
                cfg.SwaggerEndpoint($"{groupName}/swagger.json", $"Service API {groupName}");
            }
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.RemoveExcludedFromEnvironment(builder.Environment);

app.Run();