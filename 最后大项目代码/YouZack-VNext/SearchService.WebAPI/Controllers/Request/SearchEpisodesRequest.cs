using FluentValidation;

namespace SearchService.WebAPI.Controllers.Request;
public record SearchEpisodesRequest(string Keyword, int PageIndex, int PageSize);

public class SearchEpisodesRequestValidator : AbstractValidator<SearchEpisodesRequest>
{
    public SearchEpisodesRequestValidator()
    {
        RuleFor(e => e.Keyword).NotNull().MinimumLength(2).MaximumLength(100);
        RuleFor(e => e.PageIndex).GreaterThan(0);//页号从1开始
        RuleFor(e => e.PageSize).GreaterThanOrEqualTo(5);
    }
}
