using Microsoft.AspNetCore.Mvc;
using Nest;
using SearchService.WebAPI.Controllers.Request;
using SearchService.WebAPI.Controllers.Response;
using SearchService.WebAPI.IndexModels;
using Zack.EventBus;

namespace SearchService.WebAPI.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class SearchController : ControllerBase
{
    private readonly IElasticClient elasticClient;
    private readonly IEventBus eventBus;

    public SearchController(IElasticClient elasticClient, IEventBus eventBus)
    {
        this.elasticClient = elasticClient;
        this.eventBus = eventBus;
    }

    [HttpGet]
    public async Task<SearchEpisodesResponse> SearchEpisodes([FromQuery] SearchEpisodesRequest req)
    {
        //页号从1开始
        int from = req.PageSize * (req.PageIndex - 1);
        string kw = req.Keyword;
        Func<QueryContainerDescriptor<Episode>, QueryContainer> query = (q) =>
                      q.Match(mq => mq.Field(f => f.CnName).Query(kw))
                      || q.Match(mq => mq.Field(f => f.EngName).Query(kw))
                      || q.Match(mq => mq.Field(f => f.PlainSubtitle).Query(kw));
        Func<HighlightDescriptor<Episode>, IHighlight> highlightSelector = h => h
            .Fields(fs => fs.Field(f => f.PlainSubtitle));
        var result = await this.elasticClient.SearchAsync<Episode>(s => s.Index("episodes").From(from)
            .Size(req.PageSize).Query(query).Highlight(highlightSelector));
        List<Episode> episodes = new List<Episode>();
        foreach (var hit in result.Hits)
        {
            string highlightedSubtitle;
            //如果没有预览内容，则显示前50个字
            if (hit.Highlight.ContainsKey("plainSubtitle"))
            {
                highlightedSubtitle = string.Join("\r\n", hit.Highlight["plainSubtitle"]);
            }
            else
            {
                highlightedSubtitle = hit.Source.PlainSubtitle.Cut(50);
            }
            var episode = hit.Source with { PlainSubtitle = highlightedSubtitle };
            episodes.Add(episode);
        }
        return new SearchEpisodesResponse(episodes, result.Total);
    }

    [HttpPut]
    public async Task<IActionResult> ReIndexAll()
    {
        //避免耦合，这里发送ReIndexAll的集成事件
        //所有向搜索系统贡献数据的系统都可以响应这个事件，重新贡献数据
        eventBus.Publish("SearchService.ReIndexAll", null);
        return Ok();
    }
}
