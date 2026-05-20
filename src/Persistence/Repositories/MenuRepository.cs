using MenuNews.Domain.Common;
using MenuNews.Domain.Entities;
using MenuNews.Domain.Interfaces;
using MenuNews.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuNews.Persistence.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly MenuNewsDbContext _context;

    public MenuRepository(MenuNewsDbContext context) => _context = context;

    public async Task<Menu?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Menus.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<PaginatedList<Menu>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Menus
            .OrderByDescending(m => m.CreatedDate)
            .AsQueryable();

        return await Task.FromResult(PaginatedList<Menu>.Create(query, pageNumber, pageSize));
    }

    public async Task<Menu> AddAsync(Menu menu, CancellationToken cancellationToken = default)
    {
        menu.CreatedDate = DateTime.UtcNow;
        await _context.Menus.AddAsync(menu, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return menu;
    }

    public async Task UpdateAsync(Menu menu, CancellationToken cancellationToken = default)
    {
        _context.Menus.Update(menu);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Menu menu, CancellationToken cancellationToken = default)
    {
        menu.IsDeleted = true;
        menu.DeletedAt = DateTime.UtcNow;
        await UpdateAsync(menu, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Menus.AnyAsync(m => m.Id == id, cancellationToken);
}
