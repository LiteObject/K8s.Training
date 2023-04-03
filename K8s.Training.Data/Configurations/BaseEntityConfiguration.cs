using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using K8s.Training.Domain.Entities;

namespace K8s.Training.Data.Configurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            string defaultBy = "System";

            // *.HasDefaultValueSql(Database.IsSqlite() ? "datetime('now', 'utc')" : "getutcdate()")

            _ = builder?.Property(p => p.Created).ValueGeneratedOnAdd().HasDefaultValueSql("getUtcDate()");
            _ = builder?.Property(p => p.CreatedBy).ValueGeneratedOnAdd().HasDefaultValue(defaultBy);

            _ = builder?.Property(p => p.Updated).ValueGeneratedOnUpdate().HasDefaultValueSql("getUtcDate()");
            _ = builder?.Property(p => p.UpdatedBy).ValueGeneratedOnUpdate().HasDefaultValue(defaultBy);
        }
    }
}
