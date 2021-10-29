
using FluentValidation;

namespace Listening.Admin.WebAPI.Albums;
public class AlbumsSortRequest
{
    public Guid[] SortedAlbumIds { get; set; }
}

public class AlbumsSortRequestValidator : AbstractValidator<AlbumsSortRequest>
{
    public AlbumsSortRequestValidator()
    {
        RuleFor(r => r.SortedAlbumIds).NotNull().NotEmpty().NotContains(Guid.Empty)
            .NotDuplicated();
    }
}
