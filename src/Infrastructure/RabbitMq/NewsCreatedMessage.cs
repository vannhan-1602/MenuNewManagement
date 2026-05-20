namespace MenuNews.Infrastructure.RabbitMq;

public class NewsCreatedMessage
{
    public int NewsId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
