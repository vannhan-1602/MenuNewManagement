using AutoMapper;
using MediatR;
using MenuNews.Application.Features.Menus.Dtos;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.Menus.Queries.GetMenuById;

public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, MenuDto>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public GetMenuByIdQueryHandler(IMenuRepository menuRepository, IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<MenuDto> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Menu {request.Id} không tồn tại.");

        return _mapper.Map<MenuDto>(menu);
    }
}
