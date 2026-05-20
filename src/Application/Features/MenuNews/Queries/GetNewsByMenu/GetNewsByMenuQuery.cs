using MediatR;
using MenuNews.Application.Features.News.Dtos;

namespace MenuNews.Application.Features.MenuNews.Queries.GetNewsByMenu;

public record GetNewsByMenuQuery(int MenuId) : IRequest<IReadOnlyList<NewsDto>>;
