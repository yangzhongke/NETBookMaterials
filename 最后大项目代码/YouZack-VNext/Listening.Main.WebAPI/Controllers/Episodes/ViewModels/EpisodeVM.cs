
using Listening.Domain.ValueObjects;

namespace Listening.Main.WebAPI.Controllers.Episodes.ViewModels;
public record EpisodeVM(Guid Id, MultilingualString Name, Guid AlbumId, Uri AudioUrl, double DurationInSecond, IEnumerable<SentenceVM>? Sentences)
{
    public static EpisodeVM? Create(Episode? e, bool loadSubtitle)
    {
        if (e == null)
        {
            return null;
        }
        if (loadSubtitle)
        {
            var sentences = e.ParseSubtitle();
            var sentenceVMs = new List<SentenceVM>(sentences.Count());
            foreach (Sentence s in sentences)
            {
                SentenceVM vm = new SentenceVM(s.StartTime.TotalSeconds, s.EndTime.TotalSeconds, s.Value);
                sentenceVMs.Add(vm);
            }
            return new EpisodeVM(e.Id, e.Name, e.AlbumId, e.AudioUrl, e.DurationInSecond, sentenceVMs);
        }
        else
        {
            return new EpisodeVM(e.Id, e.Name, e.AlbumId, e.AudioUrl, e.DurationInSecond, null);
        }
    }

    public static EpisodeVM[] Create(Episode[] items, bool loadSubtitle)
    {
        return items.Select(e => Create(e, loadSubtitle)!).ToArray();
    }
}
