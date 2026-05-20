using MediatR;
using MenuNews.Application.Features.News.Dtos;

namespace MenuNews.Application.Features.News.Commands.CreateNews;

public record CreateNewsCommand(string Title, string Content, string Author) : IRequest<NewsDto>;
