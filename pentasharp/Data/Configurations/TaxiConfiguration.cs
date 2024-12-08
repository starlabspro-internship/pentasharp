using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class TaxiConfiguration : IEntityTypeConfiguration<Taxi>
    {
        public void Configure(EntityTypeBuilder<Taxi> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureIndexes(builder);
            ConfigureDefaults(builder);
            ConfigureRelationships(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<Taxi> builder)
        {
            builder.HasKey(t => t.TaxiId);
        }

        private void ConfigureProperties(EntityTypeBuilder<Taxi> builder)
        {
            builder.Property(t => t.TaxiId)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.LicensePlate)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.DriverId)
                .IsRequired(false); // Nullable to allow for unassigned drivers

            builder.Property(t => t.Status)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.UpdatedAt);
        }

        private void ConfigureIndexes(EntityTypeBuilder<Taxi> builder)
        {
            builder.HasIndex(t => t.LicensePlate)
                .IsUnique();

            builder.HasIndex(t => t.DriverId)
                .IsUnique()
                .HasFilter("[DriverId] IS NOT NULL");
        }

        private void ConfigureDefaults(EntityTypeBuilder<Taxi> builder)
        {
            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private void ConfigureRelationships(EntityTypeBuilder<Taxi> builder)
        {
            builder.HasOne(t => t.TaxiCompany)
                .WithMany(tc => tc.Taxis)
                .HasForeignKey(t => t.TaxiCompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.TaxiReservations)
                .WithOne(tr => tr.Taxi)
                .HasForeignKey(tr => tr.TaxiId);

            builder.HasMany(t => t.TaxiBookings)
                .WithOne(tb => tb.Taxi)
                .HasForeignKey(tb => tb.TaxiId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.DriverId)
                .OnDelete(DeleteBehavior.SetNull); 
        }
    }
}