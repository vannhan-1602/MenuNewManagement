using MenuNews.Domain.Entities;

namespace MenuNews.Domain.Interfaces;

public interface IMenuNewsRepository
{
    Task AssignNewsToMenuAsync(int menuId, int newsId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NewsItem>> GetNewsByMenuIdAsync(int menuId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Menu>> GetMenusByNewsIdAsync(int newsId, CancellationToken cancellationToken = default);
}
