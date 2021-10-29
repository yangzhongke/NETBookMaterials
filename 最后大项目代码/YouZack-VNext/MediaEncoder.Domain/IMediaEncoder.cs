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
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="destFormat"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task EncodeAsync(string sourceFileName, string destFileName, string destFormat, string[]? args = null, CancellationToken cancellationToken = default);
    }
}
