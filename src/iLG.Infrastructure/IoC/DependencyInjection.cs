using dotenv.net;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Extentions;
using iLG.Infrastructure.Loggers;
using iLG.Infrastructure.Repositories;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace iLG.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Local")
            {
                DotEnv.Load(new DotEnvOptions(envFilePaths: ["./../iLG.Docker/environment/ilgapi/.env.Local"]));
                ((ConfigurationManager)configuration).AddEnvironmentVariables();
            }

            // SQL Server
            var connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<ILGDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
            services.AddScoped<IHobbyCategoryRepository, HobbyCategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();

            // MongoDB
            services.AddSingleton(provider =>
            {
                var connectionString = configuration["MongoDb:Connection"];
                var mongoClient = new MongoClient(connectionString);
                var databaseName = configuration["MongoDb:LoggingDatabase"];
                return mongoClient.GetDatabase(databaseName);
            });

            services.AddSingleton(provider =>
            {
                var mongoDatabase = provider.GetRequiredService<IMongoDatabase>();
                var collectionName = configuration["MongoDb:LoggingCollection"];
                var logCollection = mongoDatabase.GetCollection<LogEntry>(collectionName);
                return logCollection;
            });

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddMongoDBLogger(provider =>
                {
                    return provider.GetRequiredService<IMongoCollection<LogEntry>>();
                });
            });

            // Redis Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            return services;
        }
    }
}