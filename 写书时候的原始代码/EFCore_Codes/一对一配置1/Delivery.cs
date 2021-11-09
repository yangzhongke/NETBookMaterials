using System;

namespace 一对多配置1
{
    class Delivery
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public String Number { get; set; }
        public Order Order { get; set; }
        public long OrderId { get; set; }
    }
}
