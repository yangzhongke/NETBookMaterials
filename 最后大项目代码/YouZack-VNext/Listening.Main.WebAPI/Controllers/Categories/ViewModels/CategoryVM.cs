
namespace Listening.Main.WebAPI.Controllers.Categories.ViewModels;
public record CategoryVM(Guid Id, MultilingualString Name, Uri CoverUrl)
{
    public static CategoryVM? Create(Category? e)
    {
        if (e == null)
        {
            return null;
        }
        return new CategoryVM(e.Id, e.Name, e.CoverUrl);
    }

    public static CategoryVM[] Create(Category[] items)
    {
        return items.Select(e => Create(e)!).ToArray();
    }
}
