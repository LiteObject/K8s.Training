using Microsoft.EntityFrameworkCore.Metadata.Builders;
using K8s.Training.Domain.Entities;

namespace K8s.Training.Data.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            // builder.Property(p => p.Email).HasDefaultValue("");
            base.Configure(builder);
        }
    }
}
