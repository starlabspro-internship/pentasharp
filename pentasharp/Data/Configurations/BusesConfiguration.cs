using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class BusesConfiguration : IEntityTypeConfiguration<Buses>
    {
        public void Configure(EntityTypeBuilder<Buses> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureIndexes(builder);
            ConfigureDefaults(builder);
            ConfigureRelationships(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<Buses> builder) 
        {
            builder.HasKey(b => b.BusId);
        }

        private void ConfigureProperties(EntityTypeBuilder<Buses> builder)
        {
            builder.Property(b => b.BusId)
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Status)
                .IsRequired();

            builder.Property(b => b.BusNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Capacity)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(b => b.UpdatedAt);
        }

        private void ConfigureIndexes(EntityTypeBuilder<Buses> builder)
        {
            builder.HasIndex(b => b.Status);

            builder.HasIndex(b => b.BusNumber)
                 .IsUnique();
        }

        private void ConfigureDefaults(EntityTypeBuilder<Buses> builder)
        {
            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private void ConfigureRelationships(EntityTypeBuilder<Buses> builder)
        {
            builder.HasOne(b => b.BusCompany)
                .WithMany(bc => bc.Buses)
                .HasForeignKey(b => b.BusCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.BusSchedules)
                   .WithOne(bs => bs.Bus)
                   .HasForeignKey(bs => bs.BusId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}