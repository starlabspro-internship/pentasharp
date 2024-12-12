using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Entities;

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

        builder.Property(n => n.SentAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
    }

    private void ConfigureRelationships(EntityTypeBuilder<Notifications> builder)
    {
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(n => n.TaxiBooking)
            .WithMany()
            .HasForeignKey(n => n.BookingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(); 
    }
}