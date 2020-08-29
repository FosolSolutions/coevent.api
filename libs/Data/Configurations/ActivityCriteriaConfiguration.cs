using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class ActivityCriteriaConfiguration : IEntityTypeConfiguration<ActivityCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ActivityCriteria> builder)
        {
            builder.ToTable("ActivityCriteria");

            builder.HasKey(m => new { m.ActivityId, m.CriteriaId });

            builder.HasOne(m => m.Activity).WithMany(m => m.Criteria).HasForeignKey(m => m.ActivityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Criteria).WithMany().HasForeignKey(m => m.CriteriaId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
