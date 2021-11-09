using HttpClient httpClient = new HttpClient();
string s1 = await httpClient.GetStringAsync("https://www.ptpress.com.cn");
await Task.Delay(3000);
string s2 = await httpClient.GetStringAsync("https://www.rymooc.com");