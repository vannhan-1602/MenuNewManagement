using MenuNews.Domain.Common;
using MenuNews.Domain.Entities;
using MenuNews.Domain.Interfaces;
using MenuNews.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuNews.Persistence.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly MenuNewsDbContext _context;

    public NewsRepository(MenuNewsDbContext context) => _context = context;

    public async Task<NewsItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.News.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task<PaginatedList<NewsItem>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.News
            .Where(n => n.Author != null)
            .OrderByDescending(n => n.CreatedDate);

        return await Task.FromResult(PaginatedList<NewsItem>.Create(query, pageNumber, pageSize));
    }

    public async Task<NewsItem> AddAsync(NewsItem news, CancellationToken cancellationToken = default)
    {
        news.CreatedDate = DateTime.UtcNow;
        await _context.News.AddAsync(news, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return news;
    }

    public async Task UpdateAsync(NewsItem news, CancellationToken cancellationToken = default)
    {
        _context.News.Update(news);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(NewsItem news, CancellationToken cancellationToken = default)
    {
        news.IsDeleted = true;
        news.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(news, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.News.AnyAsync(n => n.Id == id, cancellationToken);
}
