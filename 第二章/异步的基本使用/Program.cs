//基本用法
/*
Console.WriteLine("before write file");
await File.WriteAllTextAsync("d:/1.txt", "hello async");
Console.WriteLine("before read file");
string s = await File.ReadAllTextAsync("d:/1.txt");
Console.WriteLine(s);*/
//错误用法：没有用await调用异步方法

string fileName = "d:/1.txt";
File.Delete(fileName);
string text = new string('a',1000000);
File.WriteAllTextAsync(fileName, text);
string s = await File.ReadAllTextAsync(fileName);
Console.WriteLine(s);

