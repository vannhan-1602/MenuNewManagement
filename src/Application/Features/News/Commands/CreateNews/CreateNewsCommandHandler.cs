using AutoMapper;
using MediatR;
using MenuNews.Application.Common.Interfaces;
using MenuNews.Application.Features.News.Dtos;
using MenuNews.Domain.Entities;
using MenuNews.Domain.Interfaces;

namespace MenuNews.Application.Features.News.Commands.CreateNews;

public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, NewsDto>
{
    private readonly INewsRepository _newsRepository;
    private readonly INewsEventPublisher _eventPublisher;
    private readonly IAuditLogService _auditLogService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateNewsCommandHandler(
        INewsRepository newsRepository,
        INewsEventPublisher eventPublisher,
        IAuditLogService auditLogService,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _newsRepository = newsRepository;
        _eventPublisher = eventPublisher;
        _auditLogService = auditLogService;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<NewsDto> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        var news = new NewsItem
        {
            Title = request.Title,
            Content = request.Content,
            Author = request.Author
        };

        var created = await _newsRepository.AddAsync(news, cancellationToken);

        // Publish RabbitMQ message khi tạo News
        await _eventPublisher.PublishNewsCreatedAsync(created.Id, created.Title, cancellationToken);

        await _auditLogService.LogAsync("CREATE_NEWS", _currentUser.UserName ?? "system",
            new { created.Id, created.Title }, cancellationToken);

        return _mapper.Map<NewsDto>(created);
    }
}
