class Leave
{
	public long Id { get; set; }
	public User Requester { get; set; }//申请者
	public User? Approver { get; set; } //审批者
	public string Remarks { get; set; } //说明
	public DateTime From { get; set; } //开始日期
	public DateTime To { get; set; } //结束日期
	public int Status { get; set; }//状态
}