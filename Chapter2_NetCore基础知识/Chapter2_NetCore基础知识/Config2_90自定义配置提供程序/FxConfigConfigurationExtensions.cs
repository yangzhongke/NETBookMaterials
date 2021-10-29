using Config2_90自定义配置提供程序;
using Microsoft.Extensions.FileProviders;
using System;

namespace Microsoft.Extensions.Configuration
{
    public static class FxConfigConfigurationExtensions
    {
        public static IConfigurationBuilder AddFxConfigFile(this IConfigurationBuilder builder, string path, bool optional=false, bool reloadOnChange=true)
        {
            return AddFxConfigFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddFxConfigFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional=false, bool reloadOnChange=true)
        {
            return builder.AddFxConfigFile(s =>
            {
                s.FileProvider = provider;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        public static IConfigurationBuilder AddFxConfigFile(this IConfigurationBuilder builder, Action<FxConfigConfigurationSource> configureSource)
        {
            return builder.Add(configureSource);
        }
    }
}
