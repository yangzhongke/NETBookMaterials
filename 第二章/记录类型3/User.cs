public record User(string UserName, string? Email, int Age)
{
    public User(string userName, int age)
        : this(userName, null, age)
    {

    }
}