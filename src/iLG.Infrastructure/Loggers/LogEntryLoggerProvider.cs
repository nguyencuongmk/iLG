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
}