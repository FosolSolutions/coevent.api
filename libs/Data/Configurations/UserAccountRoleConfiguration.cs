using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class UserAccountRoleConfiguration : IEntityTypeConfiguration<UserAccountRole>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserAccountRole> builder)
        {
            builder.ToTable("UserAccountRoles");

            builder.HasKey(m => new { m.UserId, m.AccountRoleId });

            builder.HasOne(m => m.User).WithMany(m => m.Roles).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.AccountRole).WithMany(m => m.Users).HasForeignKey(m => m.AccountRoleId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
