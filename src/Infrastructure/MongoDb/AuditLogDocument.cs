using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MenuNews.Infrastructure.MongoDb;


// Document lưu trong MongoDB - audit log mỗi thao tác CRUD.

public class AuditLogDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("action")]
    public string Action { get; set; } = string.Empty;

    [BsonElement("user")]
    public string User { get; set; } = string.Empty;

    [BsonElement("metadata")]
    public BsonDocument? Metadata { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
