using iLG.API.IoC;
using iLG.API.Settings;
using iLG.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApiServices(builder.Configuration);

// Config appsettings.json
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();

// Config AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.

await app.UseApiServices();

app.Run();