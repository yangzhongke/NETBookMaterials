using Zack.DomainCommons.Models;

namespace Listening.Domain.Entities
{
    /// <summary>
    /// Category、album、Episode都是聚合根，而不像订单是聚合根、而订单明细是子实体一样。因为订单和订单明细是整体和部分的关系，一起出现，不会把订单明细单独访问，但是Episode是可以单独访问的
    /// ，而且Episode还可以移动到其他album。因此不能在这三个实体之间引用。
    /// 具体讨论见：https://developer.aliyun.com/article/53518
    /// </summary>
    public record Album : AggregateRootEntity, IAggregateRoot
    {
        private Album() { }

        /// <summary>
        /// 用户是否可见（完善后才显示，或者已经显示了，但是发现内部有问题，就先隐藏，调整了再发布）
        /// </summary>
        public bool IsVisible { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public MultilingualString Name { get; private set; }

        /// <summary>
        /// 列表中的显示序号
        /// </summary>
        public int SequenceNumber { get; private set; }

        public Guid CategoryId { get; private set; }

        public static Album Create(Guid id, int sequenceNumber, MultilingualString name, Guid categoryId)
        {
            Album album = new Album();
            album.Id = id;
            album.SequenceNumber = sequenceNumber;
            album.Name = name;
            album.CategoryId = categoryId;
            album.IsVisible = false;//Album新建以后默认不可见，需要手动Show
            return album;
        }
        public Album ChangeSequenceNumber(int value)
        {
            this.SequenceNumber = value;
            return this;
        }

        public Album ChangeName(MultilingualString value)
        {
            this.Name = value;
            return this;
        }
        public Album Hide()
        {
            this.IsVisible = false;
            return this;
        }
        public Album Show()
        {
            this.IsVisible = true;
            return this;
        }
    }
}
