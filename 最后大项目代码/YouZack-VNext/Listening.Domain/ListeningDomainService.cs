using Listening.Domain.Entities;
using System.Threading.Tasks;
using Zack.DomainCommons.Models;

namespace Listening.Domain
{
    public class ListeningDomainService
    {
        private readonly IListeningRepository repository;

        public ListeningDomainService(IListeningRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Album> AddAlbumAsync(Guid categoryId, MultilingualString name)
        {
            int maxSeq = await repository.GetMaxSeqOfAlbumsAsync(categoryId);
            var id = Guid.NewGuid();
            return Album.Create(id, maxSeq + 1, name, categoryId);
        }

        public async Task SortAlbumsAsync(Guid categoryId, Guid[] sortedAlbumIds)
        {
            var albums = await repository.GetAlbumsByCategoryIdAsync(categoryId);
            var idsInDB = albums.Select(a => a.Id);
            if (!idsInDB.SequenceIgnoredEqual(sortedAlbumIds))
            {
                throw new Exception($"提交的待排序Id中必须是categoryId={categoryId}分类下所有的Id");
            }

            int seqNum = 1;
            //一个in语句一次性取出来更快，不过在非性能关键节点，业务语言比性能更重要
            foreach (Guid albumId in sortedAlbumIds)
            {
                var album = await repository.GetAlbumByIdAsync(albumId);
                if (album == null)
                {
                    throw new Exception($"albumId={albumId}不存在");
                }
                album.ChangeSequenceNumber(seqNum);//顺序改序号
                seqNum++;
            }
        }

        public async Task<Category> AddCategoryAsync(MultilingualString name, Uri coverUrl)
        {
            int maxSeq = await repository.GetMaxSeqOfCategoriesAsync();
            var id = Guid.NewGuid();
            return Category.Create(id, maxSeq + 1, name, coverUrl);
        }

        public async Task SortCategoriesAsync(Guid[] sortedCategoryIds)
        {
            var categories = await repository.GetCategoriesAsync();
            var idsInDB = categories.Select(a => a.Id);
            if (!idsInDB.SequenceIgnoredEqual(sortedCategoryIds))
            {
                throw new Exception("提交的待排序Id中必须是所有的分类Id");
            }
            int seqNum = 1;
            //一个in语句一次性取出来更快，不过在非性能关键节点，业务语言比性能更重要
            foreach (Guid catId in sortedCategoryIds)
            {
                var cat = await repository.GetCategoryByIdAsync(catId);
                if (cat == null)
                {
                    throw new Exception($"categoryId={catId}不存在");
                }
                cat.ChangeSequenceNumber(seqNum);//顺序改序号
                seqNum++;
            }
        }

        public async Task<Episode> AddEpisodeAsync(MultilingualString name,
            Guid albumId, Uri audioUrl, double durationInSecond,
            string subtitleType, string subtitle)
        {
            int maxSeq = await repository.GetMaxSeqOfEpisodesAsync(albumId);
            var id = Guid.NewGuid();
            /*
            Episode episode = Episode.Create(id, maxSeq + 1, name, albumId,
                audioUrl,durationInSecond, subtitleType, subtitle);*/
            var builder = new Episode.Builder();
            builder.Id(id).SequenceNumber(maxSeq + 1).Name(name).AlbumId(albumId)
                .AudioUrl(audioUrl).DurationInSecond(durationInSecond)
                .SubtitleType(subtitleType).Subtitle(subtitle);
            return builder.Build();
        }

        public async Task SortEpisodesAsync(Guid albumId, Guid[] sortedEpisodeIds)
        {
            var episodes = await repository.GetEpisodesByAlbumIdAsync(albumId);
            var idsInDB = episodes.Select(a => a.Id);
            if (!sortedEpisodeIds.SequenceIgnoredEqual(idsInDB))
            {
                throw new Exception($"提交的待排序Id中必须是albumId={albumId}专辑下所有的Id");
            }

            int seqNum = 1;
            foreach (Guid episodeId in sortedEpisodeIds)
            {
                var episode = await repository.GetEpisodeByIdAsync(episodeId);
                if (episode == null)
                {
                    throw new Exception($"episodeId={episodeId}不存在");
                }
                episode.ChangeSequenceNumber(seqNum);//顺序改序号
                seqNum++;
            }
        }
    }
}
