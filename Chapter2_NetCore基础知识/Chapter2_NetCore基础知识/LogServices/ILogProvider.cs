namespace LogServices
{
    /// <summary>
    /// 日志记录服务
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg">信息文本</param>
        public void LogError(string msg);

        /// <summary>
        /// 记录普通信息
        /// </summary>
        /// <param name="msg">信息文本</param>
        public void LogInfo(string msg);
    }
}