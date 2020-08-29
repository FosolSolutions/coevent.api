using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("UserInfo");

            builder.HasKey(m => m.UserId);

            builder.Property(m => m.UserId).ValueGeneratedNever();
            builder.Property(m => m.DisplayName).HasMaxLength(100);
            builder.Property(m => m.Title).HasMaxLength(100);
            builder.Property(m => m.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(m => m.MiddleName).HasMaxLength(100);
            builder.Property(m => m.LastName).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(1000);
            builder.Property(m => m.HomePhone).HasMaxLength(25);
            builder.Property(m => m.MobilePhone).HasMaxLength(25);
            builder.Property(m => m.WorkPhone).HasMaxLength(25);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.HomeAddress).WithMany().HasForeignKey(m => m.HomeAddressId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.WorkAddress).WithMany().HasForeignKey(m => m.WorkAddressId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.LastName, m.FirstName, m.Gender });
        }
        #endregion
    }
}
