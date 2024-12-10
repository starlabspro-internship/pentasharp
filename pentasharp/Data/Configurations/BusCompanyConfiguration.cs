using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class BusCompanyConfiguration : IEntityTypeConfiguration<BusCompany>
    {
        public void Configure(EntityTypeBuilder<BusCompany> builder) 
        { 
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureIndexes(builder);
            ConfigureDefaults(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<BusCompany> builder)
        {
            builder.HasKey(bc => bc.BusCompanyId);
        }

        private void ConfigureProperties(EntityTypeBuilder<BusCompany> builder)
        {
            builder.Property(bc => bc.BusCompanyId)
                .ValueGeneratedOnAdd();

            builder.Property(bc => bc.CompanyName)
                 .IsRequired()
                 .HasMaxLength(100);

            builder.Property(bc => bc.ContactInfo)
                .IsRequired();

            builder.Property(bc => bc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(bc => bc.UpdatedAt);
        }

        private void ConfigureIndexes(EntityTypeBuilder<BusCompany> builder)
        {
            builder.HasIndex(bc => bc.CompanyName)
                .IsUnique();
        }

        private void ConfigureDefaults(EntityTypeBuilder<BusCompany> builder)
        {
            builder.Property(bc => bc.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private void ConfigureRelationships(EntityTypeBuilder<BusCompany> builder)
        {
            builder.HasMany(bc => bc.Buses)
                .WithOne(b => b.BusCompany)
                .HasForeignKey(b => b.BusCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
