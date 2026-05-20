namespace MenuNews.Domain.Interfaces;


// Đọc audit log từ MongoDB 

public interface IAuditLogQueryService
{
    Task<IReadOnlyList<AuditLogEntry>> GetRecentAsync(int limit = 20, CancellationToken cancellationToken = default);
}

public record AuditLogEntry(string Action, string User, DateTime CreatedAt, string? MetadataJson);
