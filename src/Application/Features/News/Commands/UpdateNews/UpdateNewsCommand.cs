using MediatR;
using MenuNews.Application.Features.News.Dtos;

namespace MenuNews.Application.Features.News.Commands.UpdateNews;

public record UpdateNewsCommand(int Id, string Title, string Content, string Author) : IRequest<NewsDto>;
