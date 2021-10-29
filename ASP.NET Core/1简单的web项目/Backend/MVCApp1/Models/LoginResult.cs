namespace MVCApp1.Models
{
    public class LoginResult
    {
        public string UserName { get; set; }
        public bool IsOK { get; set; }
        public ProcessInfo[] Processes { get; set; }
    }
}
