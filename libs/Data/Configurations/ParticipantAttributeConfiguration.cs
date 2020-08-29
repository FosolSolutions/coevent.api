using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class ParticipantAttributeConfiguration : IEntityTypeConfiguration<ParticipantAttribute>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ParticipantAttribute> builder)
        {
            builder.ToTable("ParticipantAttributes");

            builder.HasKey(m => new { m.ParticipantId, m.AttributeId });

            builder.HasOne(m => m.Participant).WithMany(m => m.Attributes).HasForeignKey(m => m.ParticipantId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Attribute).WithMany().HasForeignKey(m => m.AttributeId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
