using Listening.Domain;
using Listening.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Listening.Infrastructure
{
    public class ListeningRepository : IListeningRepository
    {
        private readonly ListeningDbContext dbCtx;

        public ListeningRepository(ListeningDbContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            var album = await dbCtx.FindAsync<Category>(categoryId);
            return album;
        }

        public Task<Category[]> GetCategoriesAsync()
        {
            return dbCtx.Categories.OrderBy(e => e.SequenceNumber).ToArrayAsync();
        }

        public async Task<Album?> GetAlbumByIdAsync(Guid albumId)
        {
            var album = await dbCtx.FindAsync<Album>(albumId);
            return album;
        }

        public async Task<int> GetMaxSeqOfAlbumsAsync(Guid categoryId)
        {
            //MaxAsync(c => (int?)c.SequenceNumber) 这样可以处理一条数据都没有的问题
            int? maxSeq = await dbCtx.Query<Album>().Where(c => c.CategoryId == categoryId)
            .MaxAsync(c => (int?)c.SequenceNumber);
            return maxSeq ?? 0;
        }

        public Task<Album[]> GetAlbumsByCategoryIdAsync(Guid categoryId)
        {
            return dbCtx.Albums.OrderBy(e => e.SequenceNumber).Where(a => a.CategoryId == categoryId).ToArrayAsync();
        }

        public async Task<int> GetMaxSeqOfCategoriesAsync()
        {
            int? maxSeq = await dbCtx.Query<Category>().MaxAsync(c => (int?)c.SequenceNumber);
            return maxSeq ?? 0;
        }

        public async Task<Episode?> GetEpisodeByIdAsync(Guid episodeId)
        {
            return await dbCtx.Episodes.SingleOrDefaultAsync(e => e.Id == episodeId);
        }

        public async Task<int> GetMaxSeqOfEpisodesAsync(Guid albumId)
        {
            int? maxSeq = await dbCtx.Query<Episode>().Where(e => e.AlbumId == albumId)
                .MaxAsync(e => (int?)e.SequenceNumber);
            return maxSeq ?? 0;
        }

        public Task<Episode[]> GetEpisodesByAlbumIdAsync(Guid albumId)
        {
            return dbCtx.Episodes.OrderBy(e => e.SequenceNumber).Where(a => a.AlbumId == albumId).ToArrayAsync();
        }
    }
}
