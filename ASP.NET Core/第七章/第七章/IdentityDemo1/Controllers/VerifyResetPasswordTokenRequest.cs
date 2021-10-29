namespace IdentityDemo1.Controllers
{
    public class VerifyResetPasswordTokenRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }
    }
}
