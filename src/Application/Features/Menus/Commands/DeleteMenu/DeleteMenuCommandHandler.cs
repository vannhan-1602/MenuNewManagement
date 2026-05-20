using MediatR;
using MenuNews.Application.Common.Interfaces;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.Menus.Commands.DeleteMenu;

public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuCommand, Unit>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ICurrentUserService _currentUser;

    public DeleteMenuCommandHandler(
        IMenuRepository menuRepository,
        IAuditLogService auditLogService,
        ICurrentUserService currentUser)
    {
        _menuRepository = menuRepository;
        _auditLogService = auditLogService;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Menu {request.Id} không tồn tại.");

        await _menuRepository.SoftDeleteAsync(menu, cancellationToken);
        await _auditLogService.LogAsync("DELETE_MENU", _currentUser.UserName ?? "system",
            new { menu.Id }, cancellationToken);

        return Unit.Value;
    }
}
