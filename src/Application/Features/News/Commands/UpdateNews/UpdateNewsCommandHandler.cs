using AutoMapper;
using MediatR;
using MenuNews.Application.Common.Interfaces;
using MenuNews.Application.Features.News.Dtos;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.News.Commands.UpdateNews;

public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, NewsDto>
{
    private readonly INewsRepository _newsRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public UpdateNewsCommandHandler(
        INewsRepository newsRepository,
        IAuditLogService auditLogService,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _newsRepository = newsRepository;
        _auditLogService = auditLogService;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<NewsDto> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"News {request.Id} không tồn tại.");

        news.Title = request.Title;
        news.Content = request.Content;
        news.Author = request.Author;

        await _newsRepository.UpdateAsync(news, cancellationToken);
        await _auditLogService.LogAsync("UPDATE_NEWS", _currentUser.UserName ?? "system",
            new { news.Id }, cancellationToken);

        return _mapper.Map<NewsDto>(news);
    }
}
