using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class OpeningTagConfiguration : IEntityTypeConfiguration<OpeningTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningTag> builder)
        {
            builder.ToTable("OpeningTags");

            builder.HasKey(m => new { m.OpeningId, m.Key, m.Value });

            builder.HasOne(m => m.Opening).WithMany(m => m.Tags).HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
