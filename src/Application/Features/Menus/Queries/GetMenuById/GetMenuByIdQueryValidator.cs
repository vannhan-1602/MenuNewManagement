using FluentValidation;

namespace MenuNews.Application.Features.Menus.Queries.GetMenuById;

public class GetMenuByIdQueryValidator : AbstractValidator<GetMenuByIdQuery>
{
    public GetMenuByIdQueryValidator() => RuleFor(x => x.Id).GreaterThan(0);
}
