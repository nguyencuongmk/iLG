using iLG.Infrastructure.Data;
using iLG.Infrastructure.Repositories;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iLG.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<ILGDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
            
            services.AddScoped<IHobbyRepository, HobbyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            return services;
        }
    }
}