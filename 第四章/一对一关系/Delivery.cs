class Delivery
{
	public long Id { get; set; }
	public string CompanyName { get; set; }//快递公司名
	public String Number { get; set; }//快递单号
	public Order Order { get; set; }//订单
	public long OrderId { get; set; }//指向订单的外键
}