using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class OpeningCriteriaConfiguration : IEntityTypeConfiguration<OpeningCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningCriteria> builder)
        {
            builder.ToTable("OpeningCriteria");

            builder.HasKey(m => new { m.OpeningId, m.CriteriaId });

            builder.HasOne(m => m.Opening).WithMany(m => m.Criteria).HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Criteria).WithMany().HasForeignKey(m => m.CriteriaId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
