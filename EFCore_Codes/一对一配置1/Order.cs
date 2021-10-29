namespace 一对多配置1
{
    class Order
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Delivery Delivery { get; set; }
    }
}
