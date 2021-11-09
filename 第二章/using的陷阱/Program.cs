//以下是有问题的代码
/*
using var outStream = File.OpenWrite("e:/1.txt");
using var writer = new StreamWriter(outStream);
writer.WriteLine("hello");
string s = File.ReadAllText("e:/1.txt");
Console.WriteLine(s);*/
{
    using var outStream = File.OpenWrite("e:/1.txt");
    using var writer = new StreamWriter(outStream);
    writer.WriteLine("hello");
}
string s = File.ReadAllText("e:/1.txt");
Console.WriteLine(s);