namespace 领域事件发布的时机1
{
    public class User: BaseEntity
    {
        public Guid Id { get; init; }
        public string UserName { get; init; }
        public string Email { get; private set; }
        public string? NickName { get; private set; }
        public int? Age { get; private set; }
        public bool IsDeleted { get; private set; }
        private User()
        {
            //提供无参构造方法。避免EF Core加载数据的时候调用有参的构造方法触发领域事件
        }
        public User(string userName,string email)
        {
            this.Id = Guid.NewGuid();
            this.UserName = userName;
            this.Email = email;
            this.IsDeleted = false;
            AddDomainEvent(new UserAddedEvent(this));
        }
        public void ChangeEmail(string value)
        {
            this.Email = value;
            AddDomainEventIfAbsent(new UserUpdatedEvent(Id));
        }
        public void ChangeNickName(string? value)
        {
            this.NickName = value;
            AddDomainEventIfAbsent(new UserUpdatedEvent(Id));
        }
        public void ChangeAge(int value)
        {
            this.Age = value;
            AddDomainEventIfAbsent(new UserUpdatedEvent(Id));
        }
        public void SoftDelete()
        {
            this.IsDeleted = true;
            AddDomainEvent(new UserSoftDeletedEvent(Id));
        }
    }
}
