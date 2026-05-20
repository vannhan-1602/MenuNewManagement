using MenuNews.Application.Common.Interfaces;

namespace MenuNews.Infrastructure.Services;


// Không dùng đăng nhập — audit log ghi user mặc định.

public class CurrentUserService : ICurrentUserService
{
    public string? UserName => "system";
}
