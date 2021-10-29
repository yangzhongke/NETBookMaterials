namespace Zack.DomainCommons.Models
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }//不能写成get;protected set;否则在实现类中，这个属性不能是public http://www.itpow.com/c/2019/05/11443.asp
        void SoftDelete();
    }
}
