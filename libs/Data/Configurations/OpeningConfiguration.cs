using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class OpeningConfiguration : IEntityTypeConfiguration<Opening>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Opening> builder)
        {
            builder.ToTable("Openings");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Key).IsRequired();
            builder.Property(m => m.Name).HasMaxLength(250).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(2000);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.Activity).WithMany(m => m.Openings).HasForeignKey(m => m.ActivityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key }).IsUnique();
            builder.HasIndex(m => new { m.ActivityId, m.State, m.CriteriaRule, m.OpeningType, m.ApplicationProcess, m.Name });
        }
        #endregion
    }
}
