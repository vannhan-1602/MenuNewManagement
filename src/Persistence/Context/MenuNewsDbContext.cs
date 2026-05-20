using MenuNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuNews.Persistence.Context;


//DbContext được scaffold từ SQL Server (DB First).
public class MenuNewsDbContext : DbContext
{
    public MenuNewsDbContext(DbContextOptions<MenuNewsDbContext> options) : base(options) { }

    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<NewsItem> News => Set<NewsItem>();
    public DbSet<Domain.Entities.MenuNews> MenuNews => Set<Domain.Entities.MenuNews>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cấu hình bảng Menus
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menus");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            // Soft Delete - chỉ lấy bản ghi chưa xóa
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Cấu hình bảng News
        modelBuilder.Entity<NewsItem>(entity =>
        {
            entity.ToTable("News");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(300).IsRequired();
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Author).HasMaxLength(150).IsRequired();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Cấu hình bảng trung gian MenuNews
        modelBuilder.Entity<Domain.Entities.MenuNews>(entity =>
        {
            entity.ToTable("MenuNews");
            entity.HasKey(e => new { e.MenuId, e.NewsId });

            entity.HasOne(e => e.Menu)
                .WithMany(m => m.MenuNews)
                .HasForeignKey(e => e.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.News)
                .WithMany(n => n.MenuNews)
                .HasForeignKey(e => e.NewsId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
