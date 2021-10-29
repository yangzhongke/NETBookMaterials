
using SearchService.WebAPI.IndexModels;

namespace SearchService.WebAPI.Controllers.Response;
public record SearchEpisodesResponse(IEnumerable<Episode> Episodes, long TotalCount);
