class Order
{
	public long Id { get; set; }
	public string Name { get; set; }//商品名
	public string Address { get; set; }//收货地址
	public Delivery? Delivery { get; set; }//快递信息
}