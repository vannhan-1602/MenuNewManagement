using MenuNews.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MenuNews.Infrastructure.RabbitMq;


// Bỏ qua RabbitMQ khi chưa có Docker — vẫn tạo News bình thường.

public class NoOpNewsEventPublisher : INewsEventPublisher
{
    private readonly ILogger<NoOpNewsEventPublisher> _logger;

    public NoOpNewsEventPublisher(ILogger<NoOpNewsEventPublisher> logger) => _logger = logger;

    public Task PublishNewsCreatedAsync(int newsId, string title, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[RabbitMQ-Disabled] News Created Successfully - Id: {NewsId}, Title: {Title}", newsId, title);
        return Task.CompletedTask;
    }
}
