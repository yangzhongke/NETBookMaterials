using Listening.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Listening.Domain.Configs
{
    class EpisodeConfig : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.ToTable("T_Episodes");
            builder.HasKey(e => e.Id).IsClustered(false);//Guid类型不要聚集索引，否则会影响性能
            builder.HasIndex(e => new { e.AlbumId, e.IsDeleted });//索引不要忘了加上IsDeleted，否则会影响性能
            builder.OwnsOneMultilingualString(e => e.Name);
            //尽量用标准的、Provider无关的这些FluentAPI去配置，不要和数据库耦合
            //如果真的需要在IEntityTypeConfiguration中判断数据库类型
            //那么就定义一个接口提供DbContext属性，仿照ApplyConfigurationsFromAssembly写一个给IEntityTypeConfiguration
            //实现类注入DbContext，然后Dbcontext.Database.IsSqlServer(); 
            builder.Property(e => e.AudioUrl).HasMaxLength(1000).IsUnicode().IsRequired();
            builder.Property(e => e.Subtitle).HasMaxLength(int.MaxValue).IsUnicode().IsRequired();
            builder.Property(e => e.SubtitleType).HasMaxLength(10).IsUnicode(false).IsRequired();
        }
    }
}
