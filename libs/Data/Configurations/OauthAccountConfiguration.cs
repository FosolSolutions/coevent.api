using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class UserOathConfiguration : IEntityTypeConfiguration<OauthAccount>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OauthAccount> builder)
        {
            builder.ToTable("OauthAccounts");

            builder.HasKey(m => new { m.UserId, m.Email });

            builder.Property(m => m.UserId).ValueGeneratedNever();
            builder.Property(m => m.Email).HasMaxLength(150).IsRequired();
            builder.Property(m => m.Key).IsRequired();
            builder.Property(m => m.Issuer).HasMaxLength(100).IsRequired();
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.User).WithMany(m => m.OauthAccounts).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key }).IsUnique();
            builder.HasIndex(m => new { m.Email }).IsUnique();
        }
        #endregion
    }
}
