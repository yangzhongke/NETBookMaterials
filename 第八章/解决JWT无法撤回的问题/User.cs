using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<long>
{
	public DateTime CreationTime { get; set; }
	public string? NickName { get; set; }
	public long JWTVersion { get; set; }
}