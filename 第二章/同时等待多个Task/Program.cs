Task<string> t1 = File.ReadAllTextAsync("d:/1.txt");
Task<string> t2 = File.ReadAllTextAsync("d:/2.txt");
Task<string> t3 = File.ReadAllTextAsync("d:/3.txt");
string[] results = await Task.WhenAll(t1, t2, t3);
string s1 = results[0];
string s2 = results[1];
string s3 = results[2];