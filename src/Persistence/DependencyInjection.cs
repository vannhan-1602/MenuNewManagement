using MenuNews.Domain.Interfaces;
using MenuNews.Persistence.Context;
using MenuNews.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuNews.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MenuNewsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IMenuNewsRepository, MenuNewsRepository>();

        return services;
    }
}
