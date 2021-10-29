namespace MySQL乐观锁2
{
    class House
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string RowVer { get; set; }
    }
}
