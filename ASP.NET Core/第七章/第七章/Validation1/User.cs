using System.ComponentModel.DataAnnotations.Schema;

namespace Validation1
{
    [Table("T_Users")]
    public class User
    {
        public Guid Id { get; set; }
        public string UserName {  get; set; }
        public DateTime CreationTime { get; set; }
    }
}
