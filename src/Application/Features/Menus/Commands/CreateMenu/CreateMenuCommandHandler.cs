using AutoMapper;
using MediatR;
using MenuNews.Application.Common.Interfaces;
using MenuNews.Application.Features.Menus.Dtos;
using MenuNews.Domain.Entities;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.Menus.Commands.CreateMenu;

public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, MenuDto>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateMenuCommandHandler(
        IMenuRepository menuRepository,
        IAuditLogService auditLogService,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _menuRepository = menuRepository;
        _auditLogService = auditLogService;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<MenuDto> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = new Menu
        {
            Name = request.Name,
            Description = request.Description
        };

        var created = await _menuRepository.AddAsync(menu, cancellationToken);

        await _auditLogService.LogAsync("CREATE_MENU", _currentUser.UserName ?? "system",
            new { created.Id, created.Name }, cancellationToken);

        return _mapper.Map<MenuDto>(created);
    }
}
