using SlamCodeBlog.HidingEndpoints.Extensions;
using SlamCodeBlog.HidingEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// swagger using document filter
builder.Services.AddSwaggerGen(
    opt => opt.DocumentFilter<ExcludeOnEnvironmentsDocumentsFilter>());

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
