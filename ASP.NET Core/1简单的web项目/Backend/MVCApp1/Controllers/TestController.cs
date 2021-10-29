using Microsoft.AspNetCore.Mvc;
using MVCApp1.Models;
using System;

namespace MVCApp1.Controllers
{
    public class TestController:Controller
    {
        public IActionResult Demo1()
        {
            Person model = new Person();
            model.CreatedTime = new DateTime(1999, 9, 9);
            model.IsVIP = true;
            model.Name = "Zack";
            return View(model);
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult SubmitSearch(SearchModel model)
        {
            string word = Uri.EscapeDataString(model.Word);
            string siteName = model.SiteName;
            switch(siteName)
            {
                case "Baidu":
                    return Redirect("https://www.baidu.com/s?wd="+ word);
                case "Sogou":
                    return Redirect("https://www.sogou.com/web?query=" + word);
                case "360":
                    return Redirect("https://www.so.com/s?q=" + word);
                default:
                    throw new ArgumentOutOfRangeException(nameof(siteName)); 
            }
        }
    }
}
