class User
{
	public string UserName { get; init; }
	public int Credit { get; private set; }
	private string? passwordHash;
	public User(string userName)
	{
		this.UserName = userName;
		this.Credit = 10;
	}
	public void ChangePassword(string newValue)
	{
		if (newValue.Length < 6)
		{
			throw new ArgumentException("密码太短");
		}
		this.passwordHash = HashHelper.Hash(newValue);
	}
	public bool CheckPassword(string password)
	{
		string hash = HashHelper.Hash(password);
		return passwordHash == hash;
	}
	public void DeductCredits(int delta)
	{
		if (delta <= 0)
		{
			throw new ArgumentException("额度不能为负值");
		}
		this.Credit -= delta;
	}
	public void AddCredits(int delta)
	{
		this.Credit += delta;
	}
}