using FileService.Domain.Entities;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IFSRepository
    {
        /// <summary>
        /// 查找已经上传的相同大小以及散列值的文件记录
        /// </summary>
        /// <param name="fileSize"></param>
        /// <param name="sha256Hash"></param>
        /// <returns></returns>
        Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash);
    }
}
