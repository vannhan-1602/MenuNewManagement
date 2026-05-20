namespace MenuNews.Domain.Interfaces;


// Publish message RabbitMQ khi tạo News mới.

public interface INewsEventPublisher
{
    Task PublishNewsCreatedAsync(int newsId, string title, CancellationToken cancellationToken = default);
}
