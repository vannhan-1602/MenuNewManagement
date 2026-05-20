using MediatR;

namespace MenuNews.Application.Features.MenuNews.Commands.AssignNewsToMenu;

public record AssignNewsToMenuCommand(int MenuId, int NewsId) : IRequest<Unit>;
