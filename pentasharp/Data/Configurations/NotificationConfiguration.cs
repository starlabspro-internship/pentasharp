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
            ConfigureRelationships(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<Notifications> builder)
        {
            builder.HasKey(n => n.NotificationId);
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

        private void ConfigureRelationships(EntityTypeBuilder<Notifications> builder)
        {
            builder.HasOne(n => n.User)  // Assuming 'User' is the related entity
                .WithMany(u => u.Notifications)  // Assuming 'Notifications' collection exists in 'User' entity
                .HasForeignKey(n => n.UserId)  // Assuming 'UserId' is the foreign key
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete to avoid cycles or conflicts
        }
    }
}
