namespace 充血模型在EFCore中的实现1
{
    class Dog
    {
        public long Id { get; set; }

        private string xingming;
        //private string name;
        public string Name 
        { 
            get
            {
                Console.WriteLine("get被调用");
                return xingming;
            }
            set 
            {
                Console.WriteLine("set被调用");
                this.xingming = value; 
            } 
        }
    }
}
