using MenuNews.Domain.Entities;
using MenuNews.Domain.Interfaces;
using MenuNews.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuNews.Persistence.Repositories;

public class MenuNewsRepository : IMenuNewsRepository
{
    private readonly MenuNewsDbContext _context;

    public MenuNewsRepository(MenuNewsDbContext context) => _context = context;

    public async Task AssignNewsToMenuAsync(int menuId, int newsId, CancellationToken cancellationToken = default)
    {
        var exists = await _context.MenuNews
            .AnyAsync(mn => mn.MenuId == menuId && mn.NewsId == newsId, cancellationToken);

        if (exists) return;

        await _context.MenuNews.AddAsync(new Domain.Entities.MenuNews { MenuId = menuId, NewsId = newsId }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NewsItem>> GetNewsByMenuIdAsync(int menuId, CancellationToken cancellationToken = default)
    {
        // LINQ Join: nối MenuNews với News để lấy danh sách News theo MenuId
        var news = await (
            from mn in _context.MenuNews
            join n in _context.News on mn.NewsId equals n.Id
            where mn.MenuId == menuId
            orderby n.CreatedDate descending
            select n
        ).ToListAsync(cancellationToken);

        return news;
    }

    public async Task<IReadOnlyList<Menu>> GetMenusByNewsIdAsync(int newsId, CancellationToken cancellationToken = default)
    {
        // LINQ Join + Select: lấy Menu theo NewsId
        var menus = await (
            from mn in _context.MenuNews
            join m in _context.Menus on mn.MenuId equals m.Id
            where mn.NewsId == newsId
            select new Menu
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                CreatedDate = m.CreatedDate
            }
        ).ToListAsync(cancellationToken);

        return menus;
    }
}
