using FileService.Domain;
using FileService.Domain.Entities;

namespace FileService.Infrastructure
{
    class FSRepository : IFSRepository
    {
        private readonly FSDbContext dbContext;

        public FSRepository(FSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
        {
            return dbContext.UploadItems.FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize
            && u.FileSHA256Hash == sha256Hash);
        }
    }
}
