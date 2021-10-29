
namespace Listening.Admin.WebAPI.Episodes.Response;
public record EpisodeForFindByAlbumId(Guid Id, int SequenceNumber, MultilingualString Name, Uri AudioUrl, double DurationInSecond, DateTime CreationTime, bool IsVisible, string EncodingStatus);