using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class EventCriteriaConfiguration : IEntityTypeConfiguration<EventCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<EventCriteria> builder)
        {
            builder.ToTable("EventCriteria");

            builder.HasKey(m => new { m.EventId, m.CriteriaId });

            builder.HasOne(m => m.Event).WithMany(m => m.Criteria).HasForeignKey(m => m.EventId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Criteria).WithMany().HasForeignKey(m => m.CriteriaId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
