using AutoMapper;
using MediatR;
using MenuNews.Application.Features.News.Dtos;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.MenuNews.Queries.GetNewsByMenu;

public class GetNewsByMenuQueryHandler : IRequestHandler<GetNewsByMenuQuery, IReadOnlyList<NewsDto>>
{
    private readonly IMenuNewsRepository _menuNewsRepository;
    private readonly IMapper _mapper;

    public GetNewsByMenuQueryHandler(IMenuNewsRepository menuNewsRepository, IMapper mapper)
    {
        _menuNewsRepository = menuNewsRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<NewsDto>> Handle(GetNewsByMenuQuery request, CancellationToken cancellationToken)
    {
        var news = await _menuNewsRepository.GetNewsByMenuIdAsync(request.MenuId, cancellationToken);
        return _mapper.Map<IReadOnlyList<NewsDto>>(news);
    }
}
