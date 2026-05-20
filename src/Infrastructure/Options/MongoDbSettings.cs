namespace MenuNews.Infrastructure.Options;

public class MongoDbSettings
{
    public const string SectionName = "MongoDb";
    //Bật khi đã chạy docker compose (MongoDB container)
    public bool Enabled { get; set; } = true;
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "MenuNewsAudit";
    public string AuditCollectionName { get; set; } = "audit_logs";
}
