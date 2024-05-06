using iLG.API.Helpers;
using iLG.API.IoC;
using iLG.API.Settings;
using iLG.Infrastructure.Data.Initialization;
using iLG.Infrastructure.IoC;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Config appsettings.json
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();

// Config AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add services to the container.

builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApiServices(builder.Configuration);

var app = builder.Build();

// Khởi tạo JwtHelper với phụ thuộc từ DI Container
JwtHelper.Initialize(app.Services.GetRequiredService<IOptions<AppSettings>>());

// Configure the HTTP request pipeline.

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers =
            [
                new()
                {
                    Url = "",
                    Description = "Local"
                }
            ];
        });
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "ILG API");
    });
    await app.InitializeDatabaseAsync();
}

app.Run();