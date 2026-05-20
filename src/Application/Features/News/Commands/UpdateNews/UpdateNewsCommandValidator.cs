using FluentValidation;

namespace MenuNews.Application.Features.News.Commands.UpdateNews;

public class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).MinimumLength(5);
        RuleFor(x => x.Content).MinimumLength(20);
        RuleFor(x => x.Author).NotEmpty();
    }
}
