using FluentValidation;

namespace MenuNews.Application.Features.Menus.Queries.GetAllMenus;

public class GetAllMenusQueryValidator : AbstractValidator<GetAllMenusQuery>
{
    public GetAllMenusQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
