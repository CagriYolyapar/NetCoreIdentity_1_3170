using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreIdentity_1.Models.Entities;

namespace NetCoreIdentity_1.Models.Configurations
{
    public class ProfileConfiguration:BaseConfiguration<AppUserProfile>
    {
        public override void Configure(EntityTypeBuilder<AppUserProfile> builder)
        {
            base.Configure(builder);
        }
    }
}
