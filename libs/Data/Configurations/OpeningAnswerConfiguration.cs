using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class OpeningAnswerConfiguration : IEntityTypeConfiguration<OpeningAnswer>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningAnswer> builder)
        {
            builder.ToTable("OpeningAnswers");

            builder.HasKey(m => new { m.OpeningId, m.QuestionId, m.ParticipantId });

            builder.Property(m => m.Text).HasMaxLength(1000);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.Opening).WithMany().HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Question).WithMany().HasForeignKey(m => m.QuestionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Participant).WithMany().HasForeignKey(m => m.ParticipantId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.OpeningParticipant).WithMany(m => m.Answers).HasForeignKey(m => new { m.OpeningId, m.ParticipantId }).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.OpeningQuestion).WithMany(m => m.Answers).HasForeignKey(m => new { m.OpeningId, m.QuestionId }).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
