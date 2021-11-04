using System.IO;

namespace MediaEncoder.Domain
{
    public interface IMediaEncoder
    {
        /// <summary>
        /// 是否能处理目标为outputFormat类型的文件
        /// </summary>
        /// <param name="outputFormat"></param>
        /// <returns></returns>
        bool Accept(string outputFormat);

        /// <summary>
        /// 进行转换
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        /// <param name="destFormat"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task EncodeAsync(FileInfo sourceFile, FileInfo destFile, string destFormat, string[]? args, CancellationToken ct);
    }
}
