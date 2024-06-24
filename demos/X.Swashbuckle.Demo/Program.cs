using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using X.Swashbuckle.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Demo",
        Description = "Demo Api"
    });
    options.DocInclusionPredicate((docName, description) => true);
    options.CustomSchemaIds(type => type.FullName);

    Directory.GetFiles(AppContext.BaseDirectory, "*.xml").ToList().ForEach(file =>
    {
        options.IncludeXmlComments(file, true);
    });

    options.ShowEnumDescription();
});

builder.Services.Replace(ServiceDescriptor.Transient<ISwaggerProvider, CachingSwaggerProvider>());
builder.Services.Replace(ServiceDescriptor.Transient<IAsyncSwaggerProvider, CachingSwaggerProvider>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseAuthorization();

// app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
