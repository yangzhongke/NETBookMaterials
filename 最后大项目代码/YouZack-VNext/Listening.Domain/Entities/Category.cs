using Zack.DomainCommons.Models;

namespace Listening.Domain.Entities
{
    public class Category : AggregateRootEntity, IAggregateRoot
    {
        private Category() { }
        public int SequenceNumber { get; private set; }
        public MultilingualString Name { get; private set; }
        public Uri CoverUrl { get; private set; }

        public static Category Create(Guid id, int sequenceNumber, MultilingualString name, Uri coverUrl)
        {
            Category category = new();
            category.Id = id;
            category.SequenceNumber = sequenceNumber;
            category.Name = name;
            category.CoverUrl = coverUrl;
            //category.AddDomainEvent(new CategoryCreatedEventArgs { NewObj = category });
            return category;
        }

        public Category ChangeSequenceNumber(int value)
        {
            this.SequenceNumber = value;
            return this;
        }

        public Category ChangeName(MultilingualString value)
        {
            this.Name = value;
            return this;
        }

        public Category ChangeCoverUrl(Uri value)
        {
            this.CoverUrl = value;
            return this;
        }
    }
}
