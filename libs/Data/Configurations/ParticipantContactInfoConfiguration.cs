using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class ParticipantContactInfoConfiguration : IEntityTypeConfiguration<ParticipantContactInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ParticipantContactInfo> builder)
        {
            builder.ToTable("ParticipantContactInfo");

            builder.HasKey(m => new { m.ParticipantId, m.ContactInfoId });

            builder.HasOne(m => m.Participant).WithMany(m => m.ContactInfo).HasForeignKey(m => m.ParticipantId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.ContactInfo).WithMany(m => m.Participants).HasForeignKey(m => m.ContactInfoId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
