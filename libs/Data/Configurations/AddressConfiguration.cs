using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses"); // TODO: Add schema and make it DI configurable.

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Name).HasMaxLength(100);
            builder.Property(m => m.Address1).HasMaxLength(150);
            builder.Property(m => m.Address2).HasMaxLength(150);
            builder.Property(m => m.City).HasMaxLength(150);
            builder.Property(m => m.Province).HasMaxLength(150);
            builder.Property(m => m.Country).HasMaxLength(100);
            builder.Property(m => m.PostalCode).HasMaxLength(20);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Country, m.Province, m.City, m.PostalCode });
        }
        #endregion
    }
}
