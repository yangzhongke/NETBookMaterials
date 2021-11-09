namespace Users.Domain;
public record User : IAggregateRoot
{
    public Guid Id { get; init; }
    public PhoneNumber PhoneNumber { get; private set; }//手机号
    private string? passwordHash;//密码的散列值
    public UserAccessFail AccessFail { get; private set; }
    private User()
    {

    }
    public User(PhoneNumber phoneNumber)
    {
        Id = Guid.NewGuid();
        PhoneNumber = phoneNumber;
        //AccessFail不能为空，否则有bug
        //https://github.com/dotnet/efcore/issues/26489
        this.AccessFail = new UserAccessFail(this);
    }
    public bool HasPassword()//是否设置了密码
    {
        return !string.IsNullOrEmpty(passwordHash);
    }
    public void ChangePassword(string value)//修改密码
    {
        if (value.Length <= 3)
        {
            throw new ArgumentException("密码长度不能小于3");
        }
        passwordHash = HashHelper.ComputeMd5Hash(value);
    }

    public bool CheckPassword(string password)//检查密码是否正确
    {
        return passwordHash == HashHelper.ComputeMd5Hash(password);
    }

    public void ChangePhoneNumber(PhoneNumber phoneNumber)//修改手机号
    {
        PhoneNumber = phoneNumber;
    }
}