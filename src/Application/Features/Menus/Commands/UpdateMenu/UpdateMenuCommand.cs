using MediatR;
using MenuNews.Application.Features.Menus.Dtos;

namespace MenuNews.Application.Features.Menus.Commands.UpdateMenu;

public record UpdateMenuCommand(int Id, string Name, string? Description) : IRequest<MenuDto>;
