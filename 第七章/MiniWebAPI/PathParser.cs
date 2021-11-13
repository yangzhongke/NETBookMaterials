using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace MiniWebAPI
{
    public class PathParser
    {
        /// <summary>
        /// 解析出来控制器和Action的名字。
        /// 从请求路径中分析出来控制器的名字和操作方法的名字，
        /// 如果路径分析失败，返回值中的ok的值为false，
        /// 如果请求路径为”/Test1/Save”，
        /// 则Parse方法的返回值为(true,”Test1”,”Save”)；
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
