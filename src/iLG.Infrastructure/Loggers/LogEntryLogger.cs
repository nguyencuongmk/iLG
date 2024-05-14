using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace iLG.Infrastructure.Loggers
{
    public class LogEntryLogger<T> : ILogger<T>
    {
        private readonly string _categoryName;
        private readonly IMongoCollection<LogEntry> _logCollection;

        public LogEntryLogger(string categoryName, IMongoCollection<LogEntry> logCollection)
        {
            _categoryName = categoryName;
            _logCollection = logCollection;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null; // Không hỗ trợ scope trong ví dụ này
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true; // Luôn luôn bật log trong ví dụ này
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            var properties = GetProperties(state);

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = logLevel.ToString(),
                Message = message,
                Category = _categoryName,
                Properties = properties
            };

            _logCollection.InsertOne(logEntry);
        }

        private string? GetProperties<TState>(TState state)
        {
            if (state is LogEntryWrapper logEntryWrapper)
            {
                return logEntryWrapper.RequestBody;
            }

            return null;
        }
    }
}