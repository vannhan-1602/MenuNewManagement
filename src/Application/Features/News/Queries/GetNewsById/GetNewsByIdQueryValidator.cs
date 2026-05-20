using FluentValidation;

namespace MenuNews.Application.Features.News.Queries.GetNewsById;

public class GetNewsByIdQueryValidator : AbstractValidator<GetNewsByIdQuery>
{
    public GetNewsByIdQueryValidator() => RuleFor(x => x.Id).GreaterThan(0);
}
