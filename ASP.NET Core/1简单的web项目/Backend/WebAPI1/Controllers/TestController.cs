using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI1.Models;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //只有设定了 [HttpGet]、 [HttpPost]等的方法才能被swagger加载
        //如果项目中没有一个方法标注 [HttpXXX]，则Swagger运行时候界面会显示“Failed to load API definition”
        //https://blog.csdn.net/qq_44502145/article/details/113676839
        [HttpGet]
        public ActionResult<Person> Demo1()
        {
            Person model = new Person();
            model.CreatedTime = new DateTime(1999, 9, 9);
            model.IsVIP = true;
            model.Name = "Zack";
            return model;
        }

        [HttpPost]
        public ActionResult<string> SubmitSearch(SearchModel model)
        {
            string word = Uri.EscapeDataString(model.Word);
            string siteName = model.SiteName;
            switch (siteName)
            {
                case "Baidu":
                    return "https://www.baidu.com/s?wd=" + word;
                case "Sogou":
                    return "https://www.sogou.com/web?query=" + word;
                case "360":
                    return "https://www.so.com/s?q=" + word;
                default:
                    throw new ArgumentOutOfRangeException(nameof(siteName));
            }
        }
    }
}
