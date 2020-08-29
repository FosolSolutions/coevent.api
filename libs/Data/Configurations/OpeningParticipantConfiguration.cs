using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class OpeningParticipantConfiguration : IEntityTypeConfiguration<OpeningParticipant>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningParticipant> builder)
        {
            builder.ToTable("OpeningParticipants");

            builder.HasKey(m => new { m.OpeningId, m.ParticipantId });
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.Opening).WithMany(m => m.Participants).HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.Participant).WithMany(m => m.Openings).HasForeignKey(m => m.ParticipantId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.State });
        }
        #endregion
    }
}
