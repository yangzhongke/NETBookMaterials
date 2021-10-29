using System.Reflection;
using System.Text.Json;

namespace 简单的mvc框架
{
    public class BindingHelper
    {
        public static object?[] GetParameterValues(HttpContext httpContext, MethodInfo actionMethod)
        {
            var parameters = actionMethod.GetParameters();
            if (parameters.Length <= 0)
            {
                return new object?[0];
            }
            else if(parameters.Length>1)
            {
                throw new Exception("Action参数只能为0或1个");
            }
            //以下是parameters.Length==1，只有一个参数的情况
            //如果参数为HttpContext类型，则直接传递
            if (parameters.Single().ParameterType==typeof(HttpContext))
            {
                return new object?[]{ httpContext};
            }
            //其他类型的参数都从请求报文体进行json反序列化
            if (!httpContext.Request.HasJsonContentType())
            {
                throw new Exception("Action如果只有一个参数，则contentType必须是application/json");
            }
            //如果报文体是空，则绑定参数值为null
            if (httpContext.Request.ContentLength == 0)
            {
                return new object?[1] { null };
            }
            var reqStream = httpContext.Request.BodyReader.AsStream();
            //参数类型
            Type paramType = parameters.Single().ParameterType;
            object? paramValue = JsonSerializer.Deserialize(reqStream, paramType);
            return new object?[1] { paramValue };
        }
    }
}
