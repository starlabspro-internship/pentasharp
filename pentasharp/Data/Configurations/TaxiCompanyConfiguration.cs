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

            builder.Property(tr => tr.UserId)
                .IsRequired();

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

        private void ConfigureRelationships(EntityTypeBuilder<TaxiCompany> builder)
        {
            builder.HasOne(tc => tc.User) 
                .WithMany() 
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(tc => tc.Taxis)
              .WithOne(t => t.TaxiCompany)
              .HasForeignKey(t => t.TaxiCompanyId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(tc => tc.TaxiBookings)
                .WithOne(tb => tb.TaxiCompany)
                .HasForeignKey(tb => tb.TaxiCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(tc => tc.TaxiReservations)
                .WithOne(tr => tr.TaxiCompany)
                .HasForeignKey(tr => tr.TaxiCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureDefaults(EntityTypeBuilder<TaxiCompany> builder)
        {
            builder.Property(tc => tc.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}