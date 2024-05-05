using iLG.API.Handlers;
using iLG.API.Maps;
using iLG.API.Middleware;

namespace iLG.API.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(Mapper));
            services.AddExceptionHandler<ExceptionHandler>();

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseExceptionHandler(options => { });

            return app;
        }
    }
}
