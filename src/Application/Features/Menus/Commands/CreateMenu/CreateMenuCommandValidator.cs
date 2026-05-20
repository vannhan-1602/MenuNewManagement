using FluentValidation;

namespace MenuNews.Application.Features.Menus.Commands.CreateMenu;

public class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên Menu không được rỗng.")
            .MaximumLength(200);
    }
}
