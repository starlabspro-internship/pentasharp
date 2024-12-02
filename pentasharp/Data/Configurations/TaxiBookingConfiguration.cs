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

            builder.Property(tb => tb.TaxiCompanyId)
                .IsRequired();

            builder.Property(tb => tb.PickupLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tb => tb.DropoffLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tb => tb.BookingTime)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(tb => tb.TripStartTime)
                .IsRequired(false);

            builder.Property(tb => tb.TripEndTime)
                .IsRequired(false);

            builder.Property(tb => tb.Fare)
                .HasPrecision(10, 2)
                .IsRequired(false);

            builder.Property(tb => tb.PassangerCount)
                .IsRequired();

            builder.Property(tb => tb.TaxiId)
                .IsRequired(false); 

            builder.Property(tb => tb.Status)
                .IsRequired();
        }

        private void ConfigureDefaults(EntityTypeBuilder<TaxiBookings> builder)
        {
            builder.Property(tb => tb.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(tb => tb.UpdatedAt)
                .IsRequired(false);
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