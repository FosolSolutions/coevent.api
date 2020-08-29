using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder.ToTable("UserSettings");

            builder.HasKey(m => new { m.UserId, m.Key });

            builder.Property(m => m.UserId).ValueGeneratedNever();
            builder.Property(m => m.Key).HasMaxLength(50).IsRequired();
            builder.Property(m => m.Value).HasMaxLength(500).IsRequired();
            builder.Property(m => m.ValueType).HasMaxLength(250).IsRequired();
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.User).WithMany(m => m.Settings).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key, m.Value });
        }
        #endregion
    }
}
