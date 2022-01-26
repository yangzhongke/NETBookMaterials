using System;
using Zack.DomainCommons.Models;

namespace FileService.Domain.Entities
{
    public record UploadedItem : BaseEntity, IHasCreationTime
    {
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 文件大小（尺寸为字节）
        /// </summary>
        public long FileSizeInBytes { get; private set; }
        /// <summary>
        /// 用户上传的原始文件名（没有路径）
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 两个文件的大小和散列值（SHA256）都相同的概率非常小。因此只要大小和SHA256相同，就认为是相同的文件。
        /// SHA256的碰撞的概率比MD5低很多。
        /// </summary>
        public string FileSHA256Hash { get; private set; }

        /// <summary>
        /// 备份文件路径，因为可能会更换文件存储系统或者云存储供应商，因此系统会保存一份自有的路径。
        /// 备份文件一般放到内网的高速、稳定设备上，比如NAS等。
        /// </summary>
        public Uri BackupUrl { get; private set; }


        /// <summary>
        /// 上传的文件的供外部访问者访问的路径
        /// </summary>
        public Uri RemoteUrl { get; private set; }


        public static UploadedItem Create(Guid id, long fileSizeInBytes, string fileName, string fileSHA256Hash, Uri backupUrl, Uri remoteUrl)
        {
            UploadedItem item = new UploadedItem()
            {
                Id = id,
                CreationTime = DateTime.Now,
                FileName = fileName,
                FileSHA256Hash = fileSHA256Hash,
                FileSizeInBytes = fileSizeInBytes,
                BackupUrl = backupUrl,
                RemoteUrl = remoteUrl
            };
            return item;
        }
    }
}
