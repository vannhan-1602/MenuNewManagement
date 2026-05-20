using MenuNews.Domain.Common;
using MenuNews.Domain.Entities;

namespace MenuNews.Domain.Interfaces;

public interface INewsRepository
{
    Task<NewsItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaginatedList<NewsItem>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<NewsItem> AddAsync(NewsItem news, CancellationToken cancellationToken = default);
    Task UpdateAsync(NewsItem news, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(NewsItem news, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
