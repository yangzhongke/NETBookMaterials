using Microsoft.Extensions.Configuration;

namespace Config2_90自定义配置提供程序
{
    public class FxConfigConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);//处理Path等默认值问题
            return new FxConfigConfigurationProvider(this);
        }
    }
}