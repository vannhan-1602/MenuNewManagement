using MediatR;
using MenuNews.Application.Common.Interfaces;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.News.Commands.DeleteNews;

public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, Unit>
{
    private readonly INewsRepository _newsRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ICurrentUserService _currentUser;

    public DeleteNewsCommandHandler(
        INewsRepository newsRepository,
        IAuditLogService auditLogService,
        ICurrentUserService currentUser)
    {
        _newsRepository = newsRepository;
        _auditLogService = auditLogService;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"News {request.Id} không tồn tại.");

        await _newsRepository.SoftDeleteAsync(news, cancellationToken);
        await _auditLogService.LogAsync("DELETE_NEWS", _currentUser.UserName ?? "system",
            new { news.Id }, cancellationToken);

        return Unit.Value;
    }
}
