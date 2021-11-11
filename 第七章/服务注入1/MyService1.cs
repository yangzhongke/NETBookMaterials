namespace 服务注入1;
public class MyService1
{
	public IEnumerable<string> GetNames()
	{
		return new string[] { "Tom", "Zack", "Jack" };
	}
}