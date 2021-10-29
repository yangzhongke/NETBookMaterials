using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreDemo1
{
    class BookEntityConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            /*
            
            builder.ToTable("T_Books").HasIndex(b => b.Title).IsUnique().IsClustered();
            builder.Ignore(b => b.PubTime);
            builder.Property("Number").HasColumnName("No");
            builder.Property(b => b.AuthorName).HasMaxLength(20).HasColumnName("AName").IsRequired();
builder.Property(b => b.Price).HasColumnName("BookPrice").HasDefaultValue(9.9);*/
            //builder.Property(e => e.Id).UseIdentityColumn();
            //builder.HasKey(e => e.Id);
            //builder.Property(e => e.Title).HasMaxLength(50).IsRequired();
            // builder.Property(e => e.AuthorName).HasMaxLength(20).IsRequired();
            builder.ToTable("T_Books");
            builder.HasQueryFilter(b=>b.IsDeleted==false);
            
        }
    }
}
