using MediatR;

namespace MenuNews.Application.Features.News.Commands.DeleteNews;

public record DeleteNewsCommand(int Id) : IRequest<Unit>;
