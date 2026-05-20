using FluentValidation;

namespace MenuNews.Application.Features.MenuNews.Queries.GetNewsByMenu;

public class GetNewsByMenuQueryValidator : AbstractValidator<GetNewsByMenuQuery>
{
    public GetNewsByMenuQueryValidator() => RuleFor(x => x.MenuId).GreaterThan(0);
}
