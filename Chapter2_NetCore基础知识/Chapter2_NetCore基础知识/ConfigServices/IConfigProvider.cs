namespace ConfigServices
{
    /// <summary>
    /// 读取配置的服务
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// 获取名字为name的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns>如果存在，则返回值，否则返回null</returns>
        public string GetValue(string name);
    }
}