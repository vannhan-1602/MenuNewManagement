using FluentValidation;

namespace MenuNews.Application.Features.News.Commands.CreateNews;

public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(5).WithMessage("Title phải có tối thiểu 5 ký tự.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(20).WithMessage("Content phải có tối thiểu 20 ký tự.");

        RuleFor(x => x.Author).NotEmpty();
    }
}
