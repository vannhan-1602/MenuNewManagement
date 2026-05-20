using Grpc.Core;
using MediatR;
using MenuNews.Api.Grpc;
using MenuNews.Application.Features.News.Queries.GetAllNews;
using MenuNews.Application.Features.News.Queries.GetNewsById;

namespace MenuNews.Api.GrpcServices;

//gRPC Service - gọi MediatR Query handlers (CQRS read side).
public class NewsGrpcService : NewsGrpc.NewsGrpcBase
{
    private readonly IMediator _mediator;

    public NewsGrpcService(IMediator mediator) => _mediator = mediator;

    public override async Task<NewsGrpcResponse> GetNewsById(GetNewsByIdRequest request, ServerCallContext context)
    {
        var news = await _mediator.Send(new GetNewsByIdQuery(request.Id), context.CancellationToken);

        return new NewsGrpcResponse
        {
            Id = news.Id,
            Title = news.Title,
            Content = news.Content,
            Author = news.Author,
            CreatedDate = news.CreatedDate.ToString("O")
        };
    }

    public override async Task<GetAllNewsResponse> GetAllNews(GetAllNewsRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new GetAllNewsQuery(request.PageNumber > 0 ? request.PageNumber : 1,
                                request.PageSize > 0 ? request.PageSize : 10),
            context.CancellationToken);

        var response = new GetAllNewsResponse { TotalCount = result.TotalCount };
        response.Items.AddRange(result.Items.Select(n => new NewsGrpcResponse
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            Author = n.Author,
            CreatedDate = n.CreatedDate.ToString("O")
        }));

        return response;
    }
}
