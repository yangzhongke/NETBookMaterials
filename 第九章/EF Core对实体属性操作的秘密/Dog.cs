/*
class Dog
{
    public long Id { get; set; }
    private string name;
    public string Name
    {
        get
        {
            Console.WriteLine("get被调用");
            return name;
        }
        set
        {
            Console.WriteLine("set被调用");
            this.name = value;
        }
    }
}*/

class Dog
{
    public long Id { get; set; }
    private string xingming;
    public string Name
    {
        get
        {
            Console.WriteLine("get被调用");
            return xingming;
        }
        set
        {
            Console.WriteLine("set被调用");
            this.xingming = value;
        }
    }
}