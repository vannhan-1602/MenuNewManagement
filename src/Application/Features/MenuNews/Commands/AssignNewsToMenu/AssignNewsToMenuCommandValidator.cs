using FluentValidation;

namespace MenuNews.Application.Features.MenuNews.Commands.AssignNewsToMenu;

public class AssignNewsToMenuCommandValidator : AbstractValidator<AssignNewsToMenuCommand>
{
    public AssignNewsToMenuCommandValidator()
    {
        RuleFor(x => x.MenuId).GreaterThan(0);
        RuleFor(x => x.NewsId).GreaterThan(0);
    }
}
