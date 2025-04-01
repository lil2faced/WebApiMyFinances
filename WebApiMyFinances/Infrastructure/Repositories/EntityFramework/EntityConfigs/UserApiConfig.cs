using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework.Entities.Security;

namespace WebApiMyFinances.Infrastructure.Repositories.EntityFramework.EntityConfigs
{
    public class UserApiConfig : IEntityTypeConfiguration<UserAPI>
    {
        public void Configure(EntityTypeBuilder<UserAPI> builder)
        {
            builder.HasOne(p => p.Role)
                .WithMany(s => s.ApiUsers)
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
