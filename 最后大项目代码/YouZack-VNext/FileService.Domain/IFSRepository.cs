using FileService.Domain.Entities;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IFSRepository
    {
        Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash);

    }
}
