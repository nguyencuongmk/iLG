using iLG.API.Handlers;
using iLG.API.IoC;
using iLG.API.Middleware;
using iLG.Infrastructure.IoC;
using iLG.Infrastructure.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApiServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitialiseDatabaseAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<LoggingMiddleware>();
app.UseExceptionHandler(options => { });

app.Run();