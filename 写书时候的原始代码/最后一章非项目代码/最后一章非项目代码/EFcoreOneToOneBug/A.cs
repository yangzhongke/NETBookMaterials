using System.ComponentModel.DataAnnotations.Schema;

namespace EFcoreOneToOneBug
{
    class A
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public B? B { get; set; }
        public Guid? BId { get; set; }
    }
}
