using AutoMapper;
using MediatR;
using MenuNews.Application.Features.Menus.Dtos;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.MenuNews.Queries.GetMenusByNews;

public class GetMenusByNewsQueryHandler : IRequestHandler<GetMenusByNewsQuery, IReadOnlyList<MenuDto>>
{
    private readonly IMenuNewsRepository _menuNewsRepository;
    private readonly IMapper _mapper;

    public GetMenusByNewsQueryHandler(IMenuNewsRepository menuNewsRepository, IMapper mapper)
    {
        _menuNewsRepository = menuNewsRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MenuDto>> Handle(GetMenusByNewsQuery request, CancellationToken cancellationToken)
    {
        var menus = await _menuNewsRepository.GetMenusByNewsIdAsync(request.NewsId, cancellationToken);
        return _mapper.Map<IReadOnlyList<MenuDto>>(menus);
    }
}
