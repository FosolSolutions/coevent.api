using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.IsRequired).IsRequired();
            builder.Property(m => m.Caption).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Text).HasMaxLength(500).IsRequired();
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.Account).WithMany(m => m.Questions).HasForeignKey(m => m.AccountId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.IsRequired });
        }
        #endregion
    }
}
