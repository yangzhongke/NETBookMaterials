namespace EFcoreOneToOneBug
{
    class B
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public A A { get; set; }
        public Guid AId { get; set; }
    }
}
