namespace MenuNews.Domain.Interfaces;


//Ghi audit log vào MongoDB khi có thao tác CREATE/UPDATE/DELETE.

public interface IAuditLogService
{
    Task LogAsync(string action, string user, object? metadata = null, CancellationToken cancellationToken = default);
}
