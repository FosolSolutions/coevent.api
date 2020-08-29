using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class OpeningQuestionConfiguration : IEntityTypeConfiguration<OpeningQuestion>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningQuestion> builder)
        {
            builder.ToTable("OpeningQuestions");

            builder.HasKey(m => new { m.OpeningId, m.QuestionId });

            builder.HasOne(m => m.Opening).WithMany(m => m.Questions).HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Question).WithMany().HasForeignKey(m => m.QuestionId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
