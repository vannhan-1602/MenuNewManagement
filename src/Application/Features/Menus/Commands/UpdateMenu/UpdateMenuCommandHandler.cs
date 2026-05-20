using AutoMapper;
using MediatR;
using MenuNews.Application.Common.Interfaces;
using MenuNews.Application.Features.Menus.Dtos;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.Menus.Commands.UpdateMenu;

public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, MenuDto>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public UpdateMenuCommandHandler(
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

    public async Task<MenuDto> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Menu {request.Id} không tồn tại.");

        menu.Name = request.Name;
        menu.Description = request.Description;

        await _menuRepository.UpdateAsync(menu, cancellationToken);
        await _auditLogService.LogAsync("UPDATE_MENU", _currentUser.UserName ?? "system",
            new { menu.Id }, cancellationToken);

        return _mapper.Map<MenuDto>(menu);
    }
}
