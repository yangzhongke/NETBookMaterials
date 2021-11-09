using 贫血模型1;

User u1 = new User();
u1.UserName = "yzk";
u1.Credit = 10;
u1.PasswordHash = HashHelper.Hash("123456");
string pwd = Console.ReadLine();
if(HashHelper.Hash(pwd)==u1.PasswordHash)
{
    u1.Credit += 5;//登录增加5个积分
    Console.WriteLine("登录成功");
}
else
{
    if (u1.Credit < 3)
    {
        Console.WriteLine("积分不足，无法扣减");
    }
    else
    {
        u1.Credit -= 3;//登录失败，则扣3个积分
        Console.WriteLine("登录成功");
    }
    Console.WriteLine("登录失败");
}