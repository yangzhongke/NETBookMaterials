
using StackExchange.Redis;

namespace Listening.Admin.WebAPI;
public class EncodingEpisodeHelper
{
    private readonly IConnectionMultiplexer redisConn;

    public EncodingEpisodeHelper(IConnectionMultiplexer redisConn)
    {
        this.redisConn = redisConn;
    }

    //一个kv对中保存这个albumId下所有的转码中的episodeId
    private static string GetKeyForEncodingEpisodeIdsOfAlbum(Guid albumId)
    {
        return $"Listening.EncodingEpisodeIdsOfAlbum.{albumId}";
    }
    private static string GetStatusKeyForEpisode(Guid episodeId)
    {
        string redisKey = $"Listening.EncodingEpisode.{episodeId}";
        return redisKey;
    }

    /// <summary>
    /// 增加待转码的任务的详细信息
    /// </summary>
    /// <param name="albumId"></param>
    /// <param name="episode"></param>
    /// <returns></returns>
    public async Task AddEncodingEpisodeAsync(Guid episodeId, EncodingEpisodeInfo episode)
    {
        string redisKeyForEpisode = GetStatusKeyForEpisode(episodeId);
        var db = redisConn.GetDatabase();
        await db.StringSetAsync(redisKeyForEpisode, episode.ToJsonString());//保存转码任务详细信息，供完成后插入数据库
        string keyForEncodingEpisodeIdsOfAlbum = GetKeyForEncodingEpisodeIdsOfAlbum(episode.AlbumId);
        await db.SetAddAsync(keyForEncodingEpisodeIdsOfAlbum, episodeId.ToString());//保存这个album下所有待转码的episodeId
    }

    /// <summary>
    /// 获取这个albumId下所有转码任务
    /// </summary>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Guid>> GetEncodingEpisodeIdsAsync(Guid albumId)
    {
        string keyForEncodingEpisodeIdsOfAlbum = GetKeyForEncodingEpisodeIdsOfAlbum(albumId);
        var db = redisConn.GetDatabase();
        var values = await db.SetMembersAsync(keyForEncodingEpisodeIdsOfAlbum);
        return values.Select(v => Guid.Parse(v));
    }

    /// <summary>
    /// 删除一个Episode任务
    /// </summary>
    /// <param name="db"></param>
    /// <param name="episodeId"></param>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task RemoveEncodingEpisodeAsync(Guid episodeId, Guid albumId)
    {
        string redisKeyForEpisode = GetStatusKeyForEpisode(episodeId);
        var db = redisConn.GetDatabase();
        await db.KeyDeleteAsync(redisKeyForEpisode);
        string keyForEncodingEpisodeIdsOfAlbum = GetKeyForEncodingEpisodeIdsOfAlbum(albumId);
        await db.SetRemoveAsync(keyForEncodingEpisodeIdsOfAlbum, episodeId.ToString());
    }

    /// <summary>
    /// 修改Episode的转码状态
    /// </summary>
    /// <param name="db"></param>
    /// <param name="episodeId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public async Task UpdateEpisodeStatusAsync(Guid episodeId, string status)
    {
        string redisKeyForEpisode = GetStatusKeyForEpisode(episodeId);
        var db = redisConn.GetDatabase();
        string json = await db.StringGetAsync(redisKeyForEpisode);
        EncodingEpisodeInfo episode = json.ParseJson<EncodingEpisodeInfo>()!;
        episode = episode with { Status = status };
        await db.StringSetAsync(redisKeyForEpisode, episode.ToJsonString());
    }

    /// <summary>
    /// 获得Episode的转码状态
    /// </summary>
    /// <param name="db"></param>
    /// <param name="episodeId"></param>
    /// <returns></returns>
    public async Task<EncodingEpisodeInfo> GetEncodingEpisodeAsync(Guid episodeId)
    {
        string redisKey = GetStatusKeyForEpisode(episodeId);
        var db = redisConn.GetDatabase();
        string json = await db.StringGetAsync(redisKey);
        EncodingEpisodeInfo episode = json.ParseJson<EncodingEpisodeInfo>()!;
        return episode;
    }
}
