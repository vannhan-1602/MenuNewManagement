using MediatR;
using MenuNews.Application.Features.Menus.Dtos;

namespace MenuNews.Application.Features.Menus.Commands.CreateMenu;

// CQRS Command - tạo Menu mới (ghi dữ liệu).

public record CreateMenuCommand(string Name, string? Description) : IRequest<MenuDto>;
