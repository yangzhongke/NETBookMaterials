namespace CommonInitializer
{
    public class InitializerOptions
    {
        public string LogFilePath { get; set; }

        /// <summary>
        /// 入口项目的一个主要的类，会从这个类所在的程序集为根扫描所有程序进行IModuleInitializer等的遍历
        /// </summary>
        public Type StartupType { get; set; }

        private string? applicationName;

        /// <summary>
        /// 应用程序名，默认值是StartupType所在的程序集的名字，可以手动修改。
        /// 这个名字一般用于EventBus等用于标识这个应用，因此要维持“同一个项目值保持一直，不同项目不能冲突”
        /// </summary>
        public string ApplicationName
        {
            get
            {
                if (string.IsNullOrEmpty(applicationName))
                {
                    return StartupType.Assembly.GetName().Name;
                }
                else
                {
                    return applicationName;
                }
            }
            set
            {
                this.applicationName = value;
            }
        }
    }
}
