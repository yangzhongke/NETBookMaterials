public record Person(string LastName)
{
    public string FirstName { get; set; }
    public void SayHello()
    {
        Console.WriteLine($"Hello，我是{LastName} {FirstName}");
    }
}