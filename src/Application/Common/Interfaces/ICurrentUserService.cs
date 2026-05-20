namespace MenuNews.Application.Common.Interfaces;


// Lấy thông tin user hiện tại từ JWT token (dùng cho audit log).
public interface ICurrentUserService
{
    string? UserName { get; }
}
