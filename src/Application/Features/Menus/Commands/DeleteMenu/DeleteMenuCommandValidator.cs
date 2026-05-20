using FluentValidation;

namespace MenuNews.Application.Features.Menus.Commands.DeleteMenu;

public class DeleteMenuCommandValidator : AbstractValidator<DeleteMenuCommand>
{
    public DeleteMenuCommandValidator() => RuleFor(x => x.Id).GreaterThan(0);
}
