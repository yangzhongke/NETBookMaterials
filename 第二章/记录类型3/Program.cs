/*
User u1 = new User("Zack", 18);
User u2 = new User("Zack", "yzk@example.com", 18);
*/
/*
User u1 = new User("Zack", "yzk@example.com", 18);
User u2 = new User(u1.UserName, "test@example", u1.Age);
*/
User u1 = new User("Zack", "yzk@example.com", 18);
User u2 = u1 with { Email = "test@example" };
Console.WriteLine(u2);
Console.WriteLine(u1);
Console.WriteLine(Object.ReferenceEquals(u1, u2));