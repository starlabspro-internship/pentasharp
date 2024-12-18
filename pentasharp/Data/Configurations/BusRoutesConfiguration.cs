using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class BusRoutesConfiguration : IEntityTypeConfiguration<BusRoutes>
    {
        public void Configure(EntityTypeBuilder<BusRoutes> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureIndexes(builder);
        }

        private void ConfigureKeys(EntityTypeBuilder<BusRoutes> builder)
        {
            builder.HasKey(br => br.RouteId); 
        }

        private void ConfigureProperties(EntityTypeBuilder<BusRoutes> builder)
        {
            builder.Property(br => br.RouteId)
                .ValueGeneratedOnAdd();

            builder.Property(br => br.FromLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(br => br.ToLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(br => br.EstimatedDuration)
                .IsRequired();

            builder.Property(br => br.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(br => br.UpdatedAt)
                .IsRequired(false);
        }

        private void ConfigureIndexes(EntityTypeBuilder<BusRoutes> builder)
        {
            builder.HasIndex(br => br.FromLocation)
                .HasDatabaseName("IX_BusRoutes_FromLocation");

            builder.HasIndex(br => br.ToLocation)
                .HasDatabaseName("IX_BusRoutes_ToLocation");
            
            builder.HasIndex(br => new { br.FromLocation, br.ToLocation })
                .HasDatabaseName("IX_BusRoutes_FromToLocation");
        }

        private void ConfigureRelationships(EntityTypeBuilder<BusRoutes> builder)
        {
            builder.HasOne(br => br.BusCompany)
                    .WithMany(bc => bc.BusRoutes)
                    .HasForeignKey(br=>br.BusCompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}