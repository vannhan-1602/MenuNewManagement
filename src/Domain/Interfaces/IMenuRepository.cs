using MenuNews.Domain.Common;
using MenuNews.Domain.Entities;

namespace MenuNews.Domain.Interfaces;

public interface IMenuRepository
{
    Task<Menu?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaginatedList<Menu>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Menu> AddAsync(Menu menu, CancellationToken cancellationToken = default);
    Task UpdateAsync(Menu menu, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Menu menu, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
