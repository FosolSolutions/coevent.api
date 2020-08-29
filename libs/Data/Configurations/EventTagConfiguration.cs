using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class EventTagConfiguration : IEntityTypeConfiguration<EventTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<EventTag> builder)
        {
            builder.ToTable("EventTags");

            builder.HasKey(m => new { m.EventId, m.Key, m.Value });

            builder.HasOne(m => m.Event).WithMany(m => m.Tags).HasForeignKey(m => m.EventId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
