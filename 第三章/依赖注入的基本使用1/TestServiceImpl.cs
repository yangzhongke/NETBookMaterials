public class TestServiceImpl : ITestService
{
    public string Name { get; set; }
    public void SayHi()
    {
        Console.WriteLine($"Hi, I'm {Name}");
    }
}
