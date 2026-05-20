using System.Text.Json;
using MenuNews.Domain.Interfaces;
using MenuNews.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MenuNews.Infrastructure.MongoDb;

public class MongoAuditLogService : IAuditLogService
{
    private readonly IMongoCollection<AuditLogDocument> _collection;

    public MongoAuditLogService(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = settings.Value;
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        _collection = database.GetCollection<AuditLogDocument>(mongoSettings.AuditCollectionName);
    }

    public async Task LogAsync(string action, string user, object? metadata = null, CancellationToken cancellationToken = default)
    {
        var doc = new AuditLogDocument
        {
            Action = action,
            User = user,
            CreatedAt = DateTime.UtcNow,
            Metadata = metadata != null
                ? BsonDocument.Parse(JsonSerializer.Serialize(metadata))
                : null
        };

        await _collection.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }
}
