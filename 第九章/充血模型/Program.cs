User u1 = new User("yzk");
u1.ChangePassword("123456");
string pwd = Console.ReadLine();
if (u1.CheckPassword(pwd))
{
    u1.AddCredits(5);
    Console.WriteLine("登录成功");
}
else
{
    Console.WriteLine("登录失败");
}
Console.Read();