using System.Text.RegularExpressions;

namespace 简单的mvc框架
{
    public class PathParser
    {
        /// <summary>
        /// 解析出来控制器和Action的名字
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static (bool ok,string? controllerName,string? actionName) Parse(PathString pathString)
        {
            string? path = pathString.Value;
            if (path == null)
            {
                return (false, null, null);
            }
            //解析出来控制器和Action的名字
            var match = Regex.Match(path,
                "/([a-zA-Z0-9]+)/([a-zA-Z0-9]+)");
            if (!match.Success)
            {
                return (false,null,null);
            }
            string controllerName = match.Groups[1].Value;
            string actionName = match.Groups[2].Value;
            return (true,controllerName,actionName);
        }
    }
}
