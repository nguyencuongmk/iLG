using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace iLG.Infrastructure.Loggers
{
    public class LogEntry
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string? Level { get; set; }

        public string? Message { get; set; }

        public string? Category { get; set; }

        public string? Properties { get; set; }
    }

    public class LogEntryWrapper
    {
        public string Message { get; set; }

        public string RequestBody { get; set; }

        public override string ToString()
        {
            return $"{Message} - Request Body: {RequestBody}";
        }
    }
}