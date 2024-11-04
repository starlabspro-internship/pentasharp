using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notifications>
    {
        public void Configure(EntityTypeBuilder<Notifications> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<Notifications> builder)
        {
            builder.HasKey(tb => tb.BookingId);
        }

        private void ConfigureProperties(EntityTypeBuilder<Notifications> builder)
        {
            builder.Property(n => n.NotificationId)
                .ValueGeneratedOnAdd();

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(n => n.SentAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
        }

    }
}