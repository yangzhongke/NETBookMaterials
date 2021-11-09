string s1 = await ReadFileAsync(1);
Console.WriteLine(s1);
/*
async Task<string> ReadFileAsync(int num)
{
	switch (num)
	{
		case 1:
			return await File.ReadAllTextAsync("d:/1.txt");
		case 2:
			return await File.ReadAllTextAsync("d:/2.txt");
		default:
			throw new ArgumentException("num invalid");
	}
}*/
Task<string> ReadFileAsync(int num)
{
	switch (num)
	{
		case 1:
			return File.ReadAllTextAsync("d:/1.txt");
		case 2:
			return File.ReadAllTextAsync("d:/2.txt");
		default:
			throw new ArgumentException("num invalid");
	}
}