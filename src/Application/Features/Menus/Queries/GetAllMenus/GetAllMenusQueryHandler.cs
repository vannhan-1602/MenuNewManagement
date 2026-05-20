using AutoMapper;
using MediatR;
using MenuNews.Application.Features.Menus.Dtos;
using MenuNews.Domain.Common;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.Menus.Queries.GetAllMenus;

public class GetAllMenusQueryHandler : IRequestHandler<GetAllMenusQuery, PaginatedList<MenuDto>>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public GetAllMenusQueryHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MenuDto>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        var result = await _menuRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);
        var dtos = _mapper.Map<List<MenuDto>>(result.Items);
        return new PaginatedList<MenuDto>(dtos, result.TotalCount, result.PageNumber, result.PageSize);
    }
}
