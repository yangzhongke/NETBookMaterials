namespace MediaEncoder.Domain.Entities
{
    public enum ItemStatus
    {
        Ready,//任务刚创建完成
        Started,//开始处理
        Completed,//成功
        Failed,//失败
    }
}
