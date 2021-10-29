using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var claims = new List<Claim>();
/*
claims.Add(new Claim(ClaimTypes.NameIdentifier, "6"));
claims.Add(new Claim(ClaimTypes.Name, "yzk"));
claims.Add(new Claim(ClaimTypes.Role, "User"));
claims.Add(new Claim(ClaimTypes.Role, "Admin"));
claims.Add(new Claim("PassPort", "E90000082"));
string secKey = "fasdfad&9045dafz222#fadpio@0232";*/
claims.Add(new Claim(ClaimTypes.NameIdentifier, "1"));
claims.Add(new Claim(ClaimTypes.Name, "root"));
claims.Add(new Claim(ClaimTypes.Role, "Admin"));
string secKey = "111111111111111111111111111222222222222";
DateTime expires = DateTime.Now.AddDays(1);
byte[] secBytes = Encoding.UTF8.GetBytes(secKey);
var securityKey = new SymmetricSecurityKey(secBytes);
var credentials = new SigningCredentials(securityKey, 
    SecurityAlgorithms.HmacSha256Signature);
var tokenDescriptor = new JwtSecurityToken(claims:claims,
    expires: expires, signingCredentials: credentials);
string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
Console.WriteLine(jwt);