namespace MenuNews.Domain.Entities;


//Bảng trung gian Many-to-Many giữa Menu và News.

public class MenuNews
{
    public int MenuId { get; set; }
    public int NewsId { get; set; }

    public virtual Menu Menu { get; set; } = null!;
    public virtual NewsItem News { get; set; } = null!;
}
