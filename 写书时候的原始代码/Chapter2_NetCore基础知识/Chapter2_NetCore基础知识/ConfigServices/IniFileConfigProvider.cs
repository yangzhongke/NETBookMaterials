using System.IO;
using System.Linq;

namespace ConfigServices
{
    /// <summary>
    /// 从Ini格式文件中读取配置的服务
    /// </summary>
    class IniFileConfigProvider : IConfigProvider
    {
        /// <summary>
        /// Ini配置文件的路径
        /// </summary>
        public string FilePath { get; set; }
        public string GetValue(string name)
        {
            string[] lines = File.ReadAllLines(FilePath);
            //每一行按照=分隔，然后找出来=前内容等于name的那一行
            string[] keyValue = lines.Select(i => i.Split('=')).SingleOrDefault(i => i[0] == name);
            if(keyValue==null)//没有找到
            {                
                return null;
            }
            else
            {
                return keyValue[1];//=之后的为值
            }            
        }
    }
}