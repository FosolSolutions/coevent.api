using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class AccountUserConfiguration : IEntityTypeConfiguration<AccountUser>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<AccountUser> builder)
        {
            builder.ToTable("AccountUsers");

            builder.HasKey(m => new { m.AccountId, m.UserId });

            builder.HasOne(m => m.Account).WithMany(m => m.Users).HasForeignKey(m => m.AccountId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.User).WithMany(m => m.Accounts).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
