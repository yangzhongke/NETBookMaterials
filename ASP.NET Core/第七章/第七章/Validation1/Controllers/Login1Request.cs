using System.ComponentModel.DataAnnotations;

namespace Validation1.Controllers
{
    public class Login1Request
    {
        [Required]
        [EmailAddress]
        [RegularExpression("^.*@(qq|163)\\.com$", ErrorMessage = "只支持QQ和163邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        [Compare(nameof(Password2),ErrorMessage ="两次密码必须一致")]
        public string Password { get; set; }    
        public string Password2 { get; set; }
    }
}
