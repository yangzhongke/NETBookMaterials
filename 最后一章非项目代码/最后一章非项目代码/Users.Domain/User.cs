namespace Users.Domain
{
    public record User: IAggregateRoot
    {
        public Guid Id { get; init; }
        public PhoneNumber PhoneNumber { get; private set; }
        public string? PasswordHash { get; private set;}        
        public UserAccessFail AccessFail { get; private set; }
        private User()
        {
            
        }
        public User(PhoneNumber phoneNumber)
        {
            Id= Guid.NewGuid();
            PhoneNumber = phoneNumber;
            //AccessFail不能为空，否则如果AccessFail一开始为null，
            //当我们给AccessFail赋值后，立即修改AccessFail中的属性值
            //那么AccessFail的状态就从Added变成Modified了，然后保存就会出错，
            //这个应该算是EFCore的bug
            this.AccessFail = new UserAccessFail(this);
        }
        public bool HasPassword()
        {
            return !string.IsNullOrEmpty(PasswordHash);
        }
        public void ChangePassword(string value)
        {
            if(value.Length<=3)
            {
                throw new ArgumentException("密码长度不能小于3");
            }
            PasswordHash = HashHelper.ComputeMd5Hash(value);
        }

        public bool CheckPassword(string password)
        {
            return PasswordHash==HashHelper.ComputeMd5Hash(password);
        }

        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
