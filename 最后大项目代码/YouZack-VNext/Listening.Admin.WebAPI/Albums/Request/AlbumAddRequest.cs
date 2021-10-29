using FluentValidation;

namespace Listening.Admin.WebAPI.Albums;

public record AlbumAddRequest(MultilingualString Name, Guid CategoryId);

//把校验规则写到单独的文件，也是DDD的一种原则
public class AlbumAddRequestValidator : AbstractValidator<AlbumAddRequest>
{
    public AlbumAddRequestValidator(ListeningDbContext dbCtx)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name.Chinese).NotNull().Length(1, 200);
        RuleFor(x => x.Name.English).NotNull().Length(1, 200);
        ///验证CategoryId是否存在
        RuleFor(x => x.CategoryId).MustAsync((cId, ct) => dbCtx.Query<Category>().AnyAsync(c => c.Id == cId))
            .WithMessage(c => $"CategoryId={c.CategoryId}不存在");
    }
}