using MediatR;
using MenuNews.Application.Features.Menus.Dtos;

namespace MenuNews.Application.Features.MenuNews.Queries.GetMenusByNews;

public record GetMenusByNewsQuery(int NewsId) : IRequest<IReadOnlyList<MenuDto>>;
