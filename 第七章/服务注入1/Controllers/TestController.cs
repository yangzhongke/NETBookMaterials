using Microsoft.AspNetCore.Mvc;
using 服务注入1;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
	private readonly MyService1 myService1;
	public TestController(MyService1 myService1)
	{
		this.myService1 = myService1;
	}
	/*
	[HttpGet]
	public string Test()
	{
		var names = myService1.GetNames();
		return string.Join(",", names);
	}*/
	[HttpGet]
	public string Test([FromServices] MyService1 myService1, string name)
	{
		var names = myService1.GetNames();
		return string.Join(",", names) + ",hello:" + name;
	}

}