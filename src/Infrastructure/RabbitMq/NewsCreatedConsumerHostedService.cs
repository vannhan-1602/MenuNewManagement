using System.Text;
using System.Text.Json;
using MenuNews.Infrastructure.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MenuNews.Infrastructure.RabbitMq;


//nhận message từ RabbitMQ và log ra console.

public class NewsCreatedConsumerHostedService : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<NewsCreatedConsumerHostedService> _logger;

    public NewsCreatedConsumerHostedService(
        IOptions<RabbitMqSettings> settings,
        ILogger<NewsCreatedConsumerHostedService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry kết nối RabbitMQ khi container chưa sẵn sàng
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeAsync(stoppingToken);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ chưa sẵn sàng, thử lại sau");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<NewsCreatedMessage>(json);

            Console.WriteLine($"[RabbitMQ Consumer] News Created Successfully - Id: {message?.NewsId}, Title: {message?.Title}");
            _logger.LogInformation("News Created Successfully - {NewsId} - {Title}", message?.NewsId, message?.Title);

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
        };

        await channel.BasicConsumeAsync(_settings.QueueName, autoAck: false, consumer, stoppingToken);

        _logger.LogInformation("RabbitMQ Consumer đang lắng nghe queue: {Queue}", _settings.QueueName);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
