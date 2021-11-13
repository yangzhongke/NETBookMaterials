using Microsoft.Extensions.FileProviders;

namespace MiniWebAPI
{
    public class ContentTypeHelper
    {
        private static readonly Dictionary<string, string> data 
            = new (StringComparer.OrdinalIgnoreCase);
        static ContentTypeHelper()
        {
            data[".html"] = "text/html; charset=utf-8";
            data[".htm"] = "text/html; charset=utf-8";
            data[".txt"] = "text/plain; charset=utf-8";
            data[".jpg"] = "image/jpeg";
            data[".jpeg"] = "image/jpeg";
            data[".png"] = "image/png";
            data[".js"] = "application/x-javascript; charset=utf-8";
            data[".css"] = "text/css";
        }

        /// <summary>
        /// 判断一个文件是否是合法的静态文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsValid(IFileInfo file)
        {
            if(file.IsDirectory)
            {
                return false;
            }
            string extension = Path.GetExtension(file.Name);
            return data.ContainsKey(extension);
        }

        /// <summary>
        /// 获取一个文件对应的ContentType
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetContentType(IFileInfo file)
        {
            string extension = Path.GetExtension(file.Name);
            return data[extension];
        }
    }
}
