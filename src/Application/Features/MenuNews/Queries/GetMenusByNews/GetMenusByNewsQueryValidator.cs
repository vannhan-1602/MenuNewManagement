using FluentValidation;

namespace MenuNews.Application.Features.MenuNews.Queries.GetMenusByNews;

public class GetMenusByNewsQueryValidator : AbstractValidator<GetMenusByNewsQuery>
{
    public GetMenusByNewsQueryValidator() => RuleFor(x => x.NewsId).GreaterThan(0);
}
