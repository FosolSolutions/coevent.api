using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoEvent.Data.Configurations
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.ToTable("Participants");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Key).IsRequired();
            builder.Property(m => m.Email).HasMaxLength(150).IsRequired();
            builder.Property(m => m.DisplayName).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Title).HasMaxLength(100);
            builder.Property(m => m.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(m => m.MiddleName).HasMaxLength(100);
            builder.Property(m => m.LastName).HasMaxLength(100).IsRequired();
            builder.Property(m => m.HomePhone).HasMaxLength(25);
            builder.Property(m => m.MobilePhone).HasMaxLength(25);
            builder.Property(m => m.WorkPhone).HasMaxLength(25);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.HomeAddress).WithMany().HasForeignKey(m => m.HomeAddressId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.WorkAddress).WithMany().HasForeignKey(m => m.WorkAddressId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.User).WithMany(m => m.Participants).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Calendar).WithMany(m => m.Participants).HasForeignKey(m => m.CalendarId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key }).IsUnique();
            builder.HasIndex(m => new { m.CalendarId, m.DisplayName }).IsUnique();
            builder.HasIndex(m => new { m.Email, m.State });
        }
        #endregion
    }
}
