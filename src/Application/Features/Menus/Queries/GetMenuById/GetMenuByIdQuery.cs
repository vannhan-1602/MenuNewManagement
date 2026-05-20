using MediatR;
using MenuNews.Application.Features.Menus.Dtos;

namespace MenuNews.Application.Features.Menus.Queries.GetMenuById;

public record GetMenuByIdQuery(int Id) : IRequest<MenuDto>;
