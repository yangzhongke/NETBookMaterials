namespace SearchService.Domain;
public record SearchEpisodesResponse(IEnumerable<Episode> Episodes, long TotalCount);
