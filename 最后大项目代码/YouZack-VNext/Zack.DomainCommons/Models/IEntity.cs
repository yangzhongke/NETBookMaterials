using System;

namespace Zack.DomainCommons.Models
{
    public interface IEntity
    {
        public Guid Id { get; }
        //public long AutoIncId { get; }//在MySQL中，用Guid做物理主键性能非常低。因此，物理上用自增的列做主键。但是逻辑上、包括两表之间的关联都用Guid。
    }
}
