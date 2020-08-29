using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class CalendarCriteriaConfiguration : IEntityTypeConfiguration<CalendarCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<CalendarCriteria> builder)
        {
            builder.ToTable("CalendarCriteria");

            builder.HasKey(m => new { m.CalendarId, m.CriteriaId });

            builder.HasOne(m => m.Calendar).WithMany(m => m.Criteria).HasForeignKey(m => m.CalendarId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Criteria).WithMany().HasForeignKey(m => m.CriteriaId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
