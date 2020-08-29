using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class UserContactInfoConfiguration : IEntityTypeConfiguration<UserContactInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserContactInfo> builder)
        {
            builder.ToTable("UserContactInfo");

            builder.HasKey(m => new { m.UserId, m.ContactInfoId });

            builder.HasOne(m => m.User).WithMany(m => m.ContactInformation).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.ContactInfo).WithMany(m => m.Users).HasForeignKey(m => m.ContactInfoId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
