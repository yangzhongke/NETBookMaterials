/*
MyDelegate d1 = SayEnglish;
string s1 = d1(3);
Console.WriteLine(s1);
d1 = SayChinese;
string s2 = d1(5);
Console.WriteLine(s2);
static string SayEnglish(int age)
{
	return $"Hello {age}";
}
static string SayChinese(int age)
{
	return $"你好 {age}";
}
delegate string MyDelegate(int n);*/
/*
Func<int, int, string> f1 = delegate (int i1, int i2) {
    return $"{i1}+{i2}={i1 + i2}";
};
string s = f1(1, 2);
Console.WriteLine(s);*/
/*
Func<int, int, string> f1 = (i1, i2) => {
    return $"{i1}+{i2}={i1 + i2}";
};
string s = f1(1, 2);
Console.WriteLine(s);*/
MyDelegate d1 = SayEnglish;
string s1 = d1(3);
Console.WriteLine(s1);
d1 = SayChinese;
string s2 = d1(5);
Console.WriteLine(s2);
static string SayEnglish(int age)
{
    return $"Hello {age}";
}
static string SayChinese(int age)
{
    return $"你好 {age}";
}
delegate string MyDelegate(int n);
