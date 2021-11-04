using Listening.Domain.Entities;
using System.Threading.Tasks;

namespace Listening.Domain
{
    public interface IListeningRepository
    {
        public Task<Category?> GetCategoryByIdAsync(Guid categoryId);
        public Task<Category[]> GetCategoriesAsync();
        public Task<int> GetMaxSeqOfCategoriesAsync();//获取最大序号
        public Task<Album?> GetAlbumByIdAsync(Guid albumId);
        public Task<int> GetMaxSeqOfAlbumsAsync(Guid categoryId);
        public Task<Album[]> GetAlbumsByCategoryIdAsync(Guid categoryId);
        public Task<Episode?> GetEpisodeByIdAsync(Guid episodeId);
        public Task<int> GetMaxSeqOfEpisodesAsync(Guid albumId);
        public Task<Episode[]> GetEpisodesByAlbumIdAsync(Guid albumId);
    }
}
