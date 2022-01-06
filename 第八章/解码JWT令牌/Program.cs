
using System.Text;

string jwt = Console.ReadLine()!;
string[] segments = jwt.Split('.');
string head = JwtDecode(segments[0]);
string payload = JwtDecode(segments[1]);
Console.WriteLine("--------head--------");
Console.WriteLine(head);
Console.WriteLine("--------payload--------");
Console.WriteLine(payload);
string JwtDecode(string s)
{
	s = s.Replace('-', '+').Replace('_', '/');
	switch (s.Length % 4)
	{
		case 2:
			s += "==";
			break;
		case 3:
			s += "=";
			break;
	}
	var bytes = Convert.FromBase64String(s);
	return Encoding.UTF8.GetString(bytes);
}

//调用JwtSecurityTokenHandler类对JWT令牌进行解码
/*
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

string jwt = Console.ReadLine()!;
string secKey = "fasdfad&9045dafz222#fadpio@0232";
JwtSecurityTokenHandler tokenHandler = new();
TokenValidationParameters valParam = new();
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));
valParam.IssuerSigningKey = securityKey;
valParam.ValidateIssuer = false;
valParam.ValidateAudience = false;
ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(jwt,
		valParam, out SecurityToken secToken);
foreach (var claim in claimsPrincipal.Claims)
{
	Console.WriteLine($"{claim.Type}={claim.Value}");
}*/