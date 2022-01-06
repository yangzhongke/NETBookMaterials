Person p1 = new Person("Yang");
Person p2 = new Person("Yang ");
Console.WriteLine(p1);
Console.WriteLine(p1 == p2);
p1.FirstName = "Zack";
p1.SayHello();
Console.WriteLine(p1 == p2);