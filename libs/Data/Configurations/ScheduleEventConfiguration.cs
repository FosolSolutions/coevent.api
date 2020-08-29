using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class ScheduleEventConfiguration : IEntityTypeConfiguration<ScheduleEvent>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ScheduleEvent> builder)
        {
            builder.ToTable("ScheduleEvents");

            builder.HasKey(m => new { m.ScheduleId, m.EventId });

            builder.HasOne(m => m.Schedule).WithMany(m => m.Events).HasForeignKey(m => m.ScheduleId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Event).WithMany().HasForeignKey(m => m.EventId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
