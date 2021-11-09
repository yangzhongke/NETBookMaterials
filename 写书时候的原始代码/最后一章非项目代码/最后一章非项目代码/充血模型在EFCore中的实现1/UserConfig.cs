using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 充血模型在EFCore中的实现1
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property("passwordHash");//特征三
            builder.Property(u => u.Remark).HasField("remark");//特征四
            builder.Ignore(u => u.Tag);//特征五
        }
    }
}
