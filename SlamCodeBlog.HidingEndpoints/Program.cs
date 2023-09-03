using SlamCodeBlog.HidingEndpoints.Extensions;
using static SlamCodeBlog.HidingEndpoints.Swagger.SwaggerConfigurationExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// swagger using inclusion predicate
builder.Services.AddSwaggerGen(opt => 
    opt.DocInclusionPredicate((docId, apiDesc) => IsIncludedInEnvironment(docId, apiDesc, builder.Environment)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.RemoveExcludedFromEnvironment(builder.Environment);

app.Run();