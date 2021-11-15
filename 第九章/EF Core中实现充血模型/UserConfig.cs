using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property("passwordHash");//特征三
        builder.Property(u => u.Remark).HasField("remark");//特征四
        builder.Ignore(u => u.Tag);//特征五
    }
}