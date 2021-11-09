namespace 简单的mvc框架
{
    public class ContentTypeHelper
    {
        private static readonly Dictionary<string, string> data = new (StringComparer.OrdinalIgnoreCase);
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

        public static bool IsValid(FileInfo file)
        {
            return data.ContainsKey(file.Extension);
        }

        public static string GetContentType(FileInfo file)
        {
            return data[file.Extension];
        }
    }
}
