using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Config2_90自定义配置提供程序
{
    //FileConfigurationProvider定义在Microsoft.Extensions.Configuration.FileExtensions中
    public class FxConfigConfigurationProvider : FileConfigurationProvider
    {
        public FxConfigConfigurationProvider(FileConfigurationSource source):
            base(source)
        {
        }
        public override void Load(Stream stream)
        {
            //Key是大小写不敏感的
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            //获取connectionStrings节点下的连接字符串
            var nodeConnStrs = xmlDoc.SelectNodes("/configuration/connectionStrings/add");
            foreach(var nodeConnStr in nodeConnStrs.Cast<XmlNode>())
            {
                string name = nodeConnStr.Attributes["name"].Value;
                string connectionString = nodeConnStr.Attributes["connectionString"].Value;
                //以连接字符串的名字为一级节点的名字
                //connectionString、providerName等做为下一级节点的名字
                data[$"{name}:connectionString"] = connectionString;
                //providerName属性是可选的
                var providerNameProp = nodeConnStr.Attributes["providerName"];
                if(providerNameProp!=null)
                {
                    string providerName = providerNameProp.Value;
                    data[$"{name}:providerName"] = providerName;
                }
            }
            //读取appSettings下的节点
            var nodeAppSettings = xmlDoc.SelectNodes("/configuration/appSettings/add");
            foreach (var nodeAppSetting in nodeAppSettings.Cast<XmlNode>())
            {
                string key = nodeAppSetting.Attributes["key"].Value;
                key = key.Replace('.', ':');//在我们的Web.config中.也看作节点分隔符
                //因此把.替换为:，因为.net core配置系统只认:做分隔符
                string value = nodeAppSetting.Attributes["value"].Value;
                data[key] = value;
            }
            this.Data = data;
        }
    }
}