using MenuNews.Domain.Interfaces;
using MenuNews.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MenuNews.Infrastructure.MongoDb;

public class MongoAuditLogQueryService : IAuditLogQueryService
{
    private readonly IMongoCollection<AuditLogDocument> _collection;

    public MongoAuditLogQueryService(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = settings.Value;
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        _collection = database.GetCollection<AuditLogDocument>(mongoSettings.AuditCollectionName);
    }

    public async Task<IReadOnlyList<AuditLogEntry>> GetRecentAsync(int limit = 20, CancellationToken cancellationToken = default)
    {
        var docs = await _collection.Find(_ => true)
            .SortByDescending(x => x.CreatedAt)
            .Limit(limit)
            .ToListAsync(cancellationToken);

        return docs.Select(d => new AuditLogEntry(
            d.Action,
            d.User,
            d.CreatedAt,
            d.Metadata?.ToString())).ToList();
    }
}
