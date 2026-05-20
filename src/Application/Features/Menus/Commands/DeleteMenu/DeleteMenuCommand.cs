using MediatR;

namespace MenuNews.Application.Features.Menus.Commands.DeleteMenu;

public record DeleteMenuCommand(int Id) : IRequest<Unit>;
