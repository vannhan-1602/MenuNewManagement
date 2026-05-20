using AutoMapper;
using MediatR;
using MenuNews.Application.Features.News.Dtos;
using MenuNews.Domain.Common;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.News.Queries.GetAllNews;

public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, PaginatedList<NewsDto>>
{
    private readonly INewsRepository _newsRepository;
    private readonly IMapper _mapper;

    public GetAllNewsQueryHandler(INewsRepository newsRepository, IMapper mapper)
    {
        _newsRepository = newsRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<NewsDto>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
    {
        var result = await _newsRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        // LINQ GroupBy demo: nhóm news theo Author (dùng cho thống kê trong handler)
        var authorGroups = result.Items
            .GroupBy(n => n.Author)
            .Select(g => new { Author = g.Key, Count = g.Count() })
            .ToList();
        _ = authorGroups; // demo LINQ GroupBy - có thể log hoặc trả về metadata

        var dtos = _mapper.Map<List<NewsDto>>(result.Items);
        return new PaginatedList<NewsDto>(dtos, result.TotalCount, result.PageNumber, result.PageSize);
    }
}
