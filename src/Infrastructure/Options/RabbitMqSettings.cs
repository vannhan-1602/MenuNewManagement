namespace MenuNews.Infrastructure.Options;

public class RabbitMqSettings
{
    public const string SectionName = "RabbitMq";
    //Bật khi đã chạy docker compose (RabbitMQ container).
    public bool Enabled { get; set; } = true;
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string QueueName { get; set; } = "news_created_queue";
}
