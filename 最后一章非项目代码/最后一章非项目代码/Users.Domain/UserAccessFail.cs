namespace Users.Domain
{
    public record UserAccessFail
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public User User { get; init; }
        private bool lockOut;
        public DateTime? LockoutEnd { get; private set; }
        public int AccessFailedCount { get; private set; }
        private UserAccessFail()
        {

        }
        public UserAccessFail(User user)
        {
            Id = Guid.NewGuid();
            User = user;
        }
        public void Reset()
        {
            lockOut = false;
            LockoutEnd = null;
            AccessFailedCount = 0;
        }
        public bool IsLockOut()
        {
            if(lockOut)
            {
                if(LockoutEnd >= DateTime.Now)
                {
                    return true;
                }
                else
                {
                    //锁定已经到期
                    AccessFailedCount = 0;
                    LockoutEnd = null;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void Fail()
        {
            AccessFailedCount++;
            if(AccessFailedCount>=3)
            {
                lockOut = true;
                LockoutEnd = DateTime.Now.AddMinutes(5);
            }
        }
    }
}
