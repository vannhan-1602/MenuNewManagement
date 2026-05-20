using AutoMapper;
using MediatR;
using MenuNews.Application.Features.News.Dtos;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.News.Queries.GetNewsById;

public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, NewsDto>
{
    private readonly INewsRepository _newsRepository;
    private readonly IMapper _mapper;

    public GetNewsByIdQueryHandler(INewsRepository newsRepository, IMapper mapper)
    {
        _newsRepository = newsRepository;
        _mapper = mapper;
    }

    public async Task<NewsDto> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"News {request.Id} không tồn tại.");

        return _mapper.Map<NewsDto>(news);
    }
}
