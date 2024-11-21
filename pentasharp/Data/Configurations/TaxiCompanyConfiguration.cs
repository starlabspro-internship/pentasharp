using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class TaxiCompanyConfiguration : IEntityTypeConfiguration<TaxiCompany>
    {
        public void Configure(EntityTypeBuilder<TaxiCompany> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureIndexes(builder);
            ConfigureDefaults(builder);
            ConfigureRelationships(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<TaxiCompany> builder)
        {
            builder.HasKey(tc => tc.TaxiCompanyId);
        }

        private void ConfigureProperties(EntityTypeBuilder<TaxiCompany> builder)
        {
            builder.Property(tc => tc.TaxiCompanyId)
                .ValueGeneratedOnAdd();

            builder.Property(tc => tc.CompanyName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tc => tc.ContactInfo)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(tc => tc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(tc => tc.UpdatedAt);
        }

        private void ConfigureIndexes(EntityTypeBuilder<TaxiCompany> builder)
        {
            builder.HasIndex(tc => tc.CompanyName)
                .IsUnique();
        }

        private void ConfigureDefaults(EntityTypeBuilder<TaxiCompany> builder)
        {
            builder.Property(tc => tc.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        // Configure relationships between TaxiCompany and TaxiReservations
        private void ConfigureRelationships(EntityTypeBuilder<TaxiCompany> builder)
        {
            // One-to-many relationship between TaxiCompany and TaxiReservations
            builder.HasMany(tc => tc.TaxiReservations)
                .WithOne(tr => tr.TaxiCompany) // Assuming TaxiReservations has a TaxiCompany navigation property
                .HasForeignKey(tr => tr.TaxiCompanyId) // Assuming TaxiReservations has a TaxiCompanyId foreign key
                .OnDelete(DeleteBehavior.Cascade);  // Ensure deleting a company deletes related reservations
        }
    }
}
