namespace SignalRCoreTest2
{
    public static class UserManager
    {
        public static User? FindByName(string name)
        {
            return GetAll().SingleOrDefault(x => x.Name == name);   
        }
        public static User? FindById(long id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }
        public static IEnumerable<User> GetAll()
        {
            List<User> users = new List<User>();
            users.Add(new User{Id=1,Name="yzk",Password="123456"});
            users.Add(new User{Id=2,Name="tom",Password="123456"});
            users.Add(new User{Id=3,Name="jerry",Password="123456"});
            return users;
        }
    }

    public class User
    {
        public long Id {  get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
