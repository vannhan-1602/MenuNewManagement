using MediatR;
using MenuNews.Application.Features.Menus.Dtos;
using MenuNews.Domain.Common;

namespace MenuNews.Application.Features.Menus.Queries.GetAllMenus;

public record GetAllMenusQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<MenuDto>>;
