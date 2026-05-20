using MenuNews.Application.Common.Interfaces;
using MenuNews.Domain.Interfaces;
using MenuNews.Infrastructure.MongoDb;
using MenuNews.Infrastructure.Options;
using MenuNews.Infrastructure.RabbitMq;
using MenuNews.Infrastructure.Services;
using MenuNews.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuNews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.SectionName));

        var mongoEnabled = configuration.GetValue($"{MongoDbSettings.SectionName}:Enabled", true);
        var rabbitEnabled = configuration.GetValue($"{RabbitMqSettings.SectionName}:Enabled", true);

        if (mongoEnabled)
        {
            services.AddSingleton<IAuditLogService, MongoAuditLogService>();
            services.AddSingleton<IAuditLogQueryService, MongoAuditLogQueryService>();
        }
        else
        {
            services.AddSingleton<IAuditLogService, NoOpAuditLogService>();
            services.AddSingleton<IAuditLogQueryService, NoOpAuditLogQueryService>();
        }

        if (rabbitEnabled)
        {
            services.AddSingleton<INewsEventPublisher, RabbitMqNewsEventPublisher>();
            services.AddHostedService<NewsCreatedConsumerHostedService>();
        }
        else
        {
            services.AddSingleton<INewsEventPublisher, NoOpNewsEventPublisher>();
        }

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
