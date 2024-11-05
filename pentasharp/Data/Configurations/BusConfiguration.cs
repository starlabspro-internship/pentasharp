using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class BusConfiguration : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureIndexes(builder);
            ConfigureDefaults(builder);
            ConfigureRelationships(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<Bus> builder) 
        {
            builder.HasKey(b => b.BusId);
        }

        private void ConfigureProperties(EntityTypeBuilder<Bus> builder)
        {
            builder.Property(b => b.BusId)
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Capacity)
                .IsRequired();

            builder.Property(b => b.Status)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(b => b.UpdatedAt);
        }

        private void ConfigureIndexes(EntityTypeBuilder<Bus> builder)
        {
            builder.HasIndex(b => b.Status);
        }

        private void ConfigureDefaults(EntityTypeBuilder<Bus> builder)
        {
            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private void ConfigureRelationships(EntityTypeBuilder<Bus> builder)
        {
            builder.HasOne(b => b.BusCompany)
                .WithMany(bc => bc.Buses)
                .HasForeignKey(b => b.BusCompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
