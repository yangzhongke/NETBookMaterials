using System.ComponentModel.DataAnnotations.Schema;

namespace EFcoreOneToOneBug
{
    class B
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public int Age { get; set; }
        public A A { get; set; }
        public Guid AId { get; set; }
    }
}
