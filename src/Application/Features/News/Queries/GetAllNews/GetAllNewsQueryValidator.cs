using FluentValidation;

namespace MenuNews.Application.Features.News.Queries.GetAllNews;

public class GetAllNewsQueryValidator : AbstractValidator<GetAllNewsQuery>
{
    public GetAllNewsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
