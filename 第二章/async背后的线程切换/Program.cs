using System.Text;

Console.WriteLine("1-ThreadId=" + Thread.CurrentThread.ManagedThreadId);
string str = new string('a', 10000000);
await File.WriteAllTextAsync("d:/1.txt", str);
Console.WriteLine("2-ThreadId=" + Thread.CurrentThread.ManagedThreadId);
await File.WriteAllTextAsync("d:/2.txt", str);
Console.WriteLine("3-ThreadId=" + Thread.CurrentThread.ManagedThreadId);
File.WriteAllText("d:/3.txt", str);//同步写入
Console.WriteLine("4-ThreadId=" + Thread.CurrentThread.ManagedThreadId);