using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class OpeningAnswerQuestionOptionConfiguration : IEntityTypeConfiguration<OpeningAnswerQuestionOption>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningAnswerQuestionOption> builder)
        {
            builder.ToTable("OpeningAnswerQuestionOptions");

            builder.HasKey(m => new { m.OpeningId, m.QuestionId, m.ParticipantId, m.QuestionOptionId });

            builder.HasOne(m => m.Opening).WithMany().HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Question).WithMany().HasForeignKey(m => m.QuestionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Participant).WithMany().HasForeignKey(m => m.ParticipantId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.Option).WithMany().HasForeignKey(m => m.QuestionOptionId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.OpeningAnswer).WithMany(m => m.Options).HasForeignKey(m => new { m.OpeningId, m.QuestionId, m.ParticipantId }).OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
