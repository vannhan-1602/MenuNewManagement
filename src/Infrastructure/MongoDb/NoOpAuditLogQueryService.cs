using MenuNews.Domain.Interfaces;

namespace MenuNews.Infrastructure.MongoDb;

public class NoOpAuditLogQueryService : IAuditLogQueryService
{
    public Task<IReadOnlyList<AuditLogEntry>> GetRecentAsync(int limit = 20, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<AuditLogEntry>>(Array.Empty<AuditLogEntry>());
}
