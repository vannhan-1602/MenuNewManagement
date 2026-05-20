namespace MenuNews.Domain.Entities;


/// Entity Menu - một Menu có thể chứa nhiều News (quan hệ N-N qua MenuNews).

public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // quan hệ many-to-many
    public virtual ICollection<MenuNews> MenuNews { get; set; } = new List<MenuNews>();
}
