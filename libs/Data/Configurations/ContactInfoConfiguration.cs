using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            builder.ToTable("ContactInfo");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Value).HasMaxLength(250).IsRequired();

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Name, m.Category, m.Value });
        }
        #endregion
    }
}
