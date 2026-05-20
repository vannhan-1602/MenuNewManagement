using System.Text;
using System.Text.Json;
using MenuNews.Domain.Interfaces;
using MenuNews.Infrastructure.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MenuNews.Infrastructure.RabbitMq;


//publish message khi News được tạo.

public class RabbitMqNewsEventPublisher : INewsEventPublisher, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly object _lock = new();

    public RabbitMqNewsEventPublisher(IOptions<RabbitMqSettings> settings) =>
        _settings = settings.Value;

    public async Task PublishNewsCreatedAsync(int newsId, string title, CancellationToken cancellationToken = default)
    {
        await EnsureChannelAsync(cancellationToken);

        var message = new NewsCreatedMessage { NewsId = newsId, Title = title };
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = new BasicProperties { Persistent = true };
        await _channel!.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _settings.QueueName,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    private async Task EnsureChannelAsync(CancellationToken cancellationToken)
    {
        if (_channel is { IsOpen: true }) return;

        lock (_lock)
        {
            if (_channel is { IsOpen: true }) return;
        }

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
