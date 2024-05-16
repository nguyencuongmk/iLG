using iLG.Infrastructure.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace iLG.Infrastructure.Extentions
{
    public static class LoggingExtension
    {
        public static ILoggingBuilder AddMongoDBLogger(this ILoggingBuilder builder, Func<IServiceProvider, IMongoCollection<LogEntry>> collectionFactory)
        {
            builder.AddProvider(new LogEntryLoggerProvider(collectionFactory, builder.Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>()));
            return builder;
        }

        public static void WriteLog<T>(this ILogger<T> logger, LogLevel level,string message, string body)
        {
            var logEntry = new LogEntry { Message = message, Properties = body };
            logger.Log(level, 0, logEntry, null, (state, _) => state.Message);
        }
    }
}