Person p1 = new Person("Zack");
Person p2 = new Person("Zack");
Console.WriteLine(p1);
Console.WriteLine(p1 == p2);
p1.FirstName = "Yang";
p1.SayHello();
Console.WriteLine(p1 == p2);