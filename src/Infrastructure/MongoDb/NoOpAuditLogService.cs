using MenuNews.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MenuNews.Infrastructure.MongoDb;

/// <summary>
/// Ghi audit ra log khi MongoDB tắt (chạy local không cần Docker).
/// </summary>
public class NoOpAuditLogService : IAuditLogService
{
    private readonly ILogger<NoOpAuditLogService> _logger;

    public NoOpAuditLogService(ILogger<NoOpAuditLogService> logger) => _logger = logger;

    public Task LogAsync(string action, string user, object? metadata = null, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[Audit-Local] {Action} by {User} {@Metadata}", action, user, metadata);
        return Task.CompletedTask;
    }
}
