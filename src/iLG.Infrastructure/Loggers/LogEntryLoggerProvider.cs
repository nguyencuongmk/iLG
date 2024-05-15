using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace iLG.Infrastructure.Loggers
{
    public class LogEntryLoggerProvider : ILoggerProvider
    {
        private readonly Func<IServiceProvider, IMongoCollection<LogEntry>> _collectionFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LogEntryLoggerProvider(Func<IServiceProvider, IMongoCollection<LogEntry>> collectionFactory, IServiceScopeFactory serviceScopeFactory)
        {
            _collectionFactory = collectionFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public ILogger CreateLogger(string categoryName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var collection = _collectionFactory.Invoke(serviceProvider);
                return new LogEntryLogger<object>(categoryName, collection);
            }
        }

        public void Dispose()
        {
        }
    }

    public static class LoggingExtensions
    {
        public static ILoggingBuilder AddMongoDBLogger(this ILoggingBuilder builder, Func<IServiceProvider, IMongoCollection<LogEntry>> collectionFactory)
        {
            builder.AddProvider(new LogEntryLoggerProvider(collectionFactory, builder.Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>()));
            return builder;
        }

        public static void LogInformation<T>(this ILogger<T> logger, string message, string requestBody)
        {
            var logEntry = new LogEntry { Message = message, Properties = requestBody };
            logger.Log(LogLevel.Information, 0, logEntry, null, (state, _) => state.Message);
        }
    }
}