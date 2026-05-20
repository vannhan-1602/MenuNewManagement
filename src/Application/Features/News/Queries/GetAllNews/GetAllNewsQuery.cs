using MediatR;
using MenuNews.Application.Features.News.Dtos;
using MenuNews.Domain.Common;

namespace MenuNews.Application.Features.News.Queries.GetAllNews;

public record GetAllNewsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<NewsDto>>;
