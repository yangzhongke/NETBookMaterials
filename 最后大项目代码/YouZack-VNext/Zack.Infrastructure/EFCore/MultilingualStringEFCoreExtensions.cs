using System;
using System.Linq.Expressions;
using Zack.DomainCommons.Models;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders
{
    public static class MultilingualStringEFCoreExtensions
    {
        public static EntityTypeBuilder<TEntity> OwnsOneMultilingualString<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder,
            Expression<Func<TEntity, MultilingualString>> navigationExpression, bool required = true, int maxLength = 200) where TEntity : class
        {
            /*
             * The entity type 'Episode.Name#MultilingualString' is an optional dependent using table sharing without any required non shared property 
             * that could be used to identify whether the entity exists. If all nullable properties contain a null value in database then an object
             * instance won't be created in the query. Add a required property to create instances with null values for other properties or mark the
             * incoming navigation as required to always create an instance.
             */
            entityTypeBuilder.OwnsOne(navigationExpression, dp =>
            {
                dp.Property(c => c.Chinese).IsRequired(required).HasMaxLength(maxLength).IsUnicode();
                dp.Property(c => c.English).IsRequired(required).HasMaxLength(maxLength).IsUnicode();
            });
            entityTypeBuilder.Navigation(navigationExpression).IsRequired(required);
            return entityTypeBuilder;
        }
    }
}
