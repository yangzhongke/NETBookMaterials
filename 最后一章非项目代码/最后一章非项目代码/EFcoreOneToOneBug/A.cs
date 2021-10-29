namespace EFcoreOneToOneBug
{
    class A
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public B? B { get; set; }
        public Guid? BId { get; set; }
    }
}
