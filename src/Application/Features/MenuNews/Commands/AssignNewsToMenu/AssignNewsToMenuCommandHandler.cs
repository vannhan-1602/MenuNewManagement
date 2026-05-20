using MediatR;
using MenuNews.Application.Common.Interfaces;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.MenuNews.Commands.AssignNewsToMenu;

public class AssignNewsToMenuCommandHandler : IRequestHandler<AssignNewsToMenuCommand, Unit>
{
    private readonly IMenuNewsRepository _menuNewsRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ICurrentUserService _currentUser;

    public AssignNewsToMenuCommandHandler(
        IMenuNewsRepository menuNewsRepository,
        IMenuRepository menuRepository,
        INewsRepository newsRepository,
        IAuditLogService auditLogService,
        ICurrentUserService currentUser)
    {
        _menuNewsRepository = menuNewsRepository;
        _menuRepository = menuRepository;
        _newsRepository = newsRepository;
        _auditLogService = auditLogService;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AssignNewsToMenuCommand request, CancellationToken cancellationToken)
    {
        if (!await _menuRepository.ExistsAsync(request.MenuId, cancellationToken))
            throw new KeyNotFoundException($"Menu {request.MenuId} không tồn tại.");

        if (!await _newsRepository.ExistsAsync(request.NewsId, cancellationToken))
            throw new KeyNotFoundException($"News {request.NewsId} không tồn tại.");

        await _menuNewsRepository.AssignNewsToMenuAsync(request.MenuId, request.NewsId, cancellationToken);
        await _auditLogService.LogAsync("ASSIGN_NEWS_TO_MENU", _currentUser.UserName ?? "system",
            new { request.MenuId, request.NewsId }, cancellationToken);

        return Unit.Value;
    }
}
