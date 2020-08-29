using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class ActivityTagConfiguration : IEntityTypeConfiguration<ActivityTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ActivityTag> builder)
        {
            builder.ToTable("ActivityTags");

            builder.HasKey(m => new { m.ActivityId, m.Key, m.Value });

            builder.HasOne(m => m.Activity).WithMany(m => m.Tags).HasForeignKey(m => m.ActivityId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
