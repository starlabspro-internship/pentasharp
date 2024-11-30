using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class TaxiBookingConfiguration : IEntityTypeConfiguration<TaxiBookings>
    {
        public void Configure(EntityTypeBuilder<TaxiBookings> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureDefaults(builder);
            ConfigureRelationships(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<TaxiBookings> builder)
        {
            builder.HasKey(tb => tb.BookingId);
        }

        private void ConfigureProperties(EntityTypeBuilder<TaxiBookings> builder)
        {
            builder.Property(tb => tb.BookingId)
                .ValueGeneratedOnAdd();

            builder.Property(tb => tb.PickupLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tb => tb.DropoffLocation)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(tb => tb.BookingTime)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(tb => tb.TripStartTime)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(tb => tb.TripEndTime)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(tb => tb.UpdatedAt);
        }

        private void ConfigureDefaults(EntityTypeBuilder<TaxiBookings> builder)
        {
            builder.Property(tb => tb.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(tb => tb.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private void ConfigureRelationships(EntityTypeBuilder<TaxiBookings> builder)
        {
            builder.HasMany(tb => tb.Notifications)
                .WithOne(n => n.TaxiBooking)
                .HasForeignKey(n => n.BookingId)
                .IsRequired(false); 
        }
    }
}