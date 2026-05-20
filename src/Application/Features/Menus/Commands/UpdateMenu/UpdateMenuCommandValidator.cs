using FluentValidation;

namespace MenuNews.Application.Features.Menus.Commands.UpdateMenu;

public class UpdateMenuCommandValidator : AbstractValidator<UpdateMenuCommand>
{
    public UpdateMenuCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên Menu không được rỗng.");
    }
}
