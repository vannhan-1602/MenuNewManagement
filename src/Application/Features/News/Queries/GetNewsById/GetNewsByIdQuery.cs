using MediatR;
using MenuNews.Application.Features.News.Dtos;

namespace MenuNews.Application.Features.News.Queries.GetNewsById;

public record GetNewsByIdQuery(int Id) : IRequest<NewsDto>;
