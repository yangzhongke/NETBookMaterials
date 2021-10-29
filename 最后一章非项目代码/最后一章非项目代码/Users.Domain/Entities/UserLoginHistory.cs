namespace Users.Domain
{
    //因为有脱离User直接查一段时间内搜索人登陆记录的需求，所以这个时单独的聚合
    public record UserLoginHistory: IAggregateRoot
    {
        public long Id { get; init; }
        public Guid? UserId { get; init; }
        public PhoneNumber PhoneNumber { get; init; }
        public DateTime CreatedDateTime { get; init; }
        public string Messsage { get; init; }
        private UserLoginHistory()
        {

        }
        public UserLoginHistory(Guid? userId, 
            PhoneNumber phoneNumber,string message)
        {
            this.UserId = userId;
            this.PhoneNumber = phoneNumber;
            this.CreatedDateTime = DateTime.Now;
            this.Messsage = message;
        }
    }
}
