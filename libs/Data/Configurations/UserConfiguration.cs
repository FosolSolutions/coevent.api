using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Key).IsRequired();
            builder.Property(m => m.Email).HasMaxLength(150).IsRequired();
            builder.Property(m => m.RowVersion).IsRowVersion();
            builder.Property(m => m.Username).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Password).HasMaxLength(500);

            builder.HasOne(m => m.Info).WithOne(m => m.User).HasForeignKey<UserInfo>(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.DefaultAccount).WithMany().HasForeignKey(m => m.DefaultAccountId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key }).IsUnique();
            builder.HasIndex(m => new { m.Email }).IsUnique();
            builder.HasIndex(m => new { m.State });
        }
        #endregion
    }
}
