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

            builder.Property(br => br.RouteName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(br => br.StartLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(br => br.EndLocation)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(br => br.Distance)
                .IsRequired();

            builder.Property(br => br.EstimatedDuration)
                .IsRequired();

            builder.Property(br => br.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(br => br.UpdatedAt)
                .IsRequired(false); // Nullable for optional updates
        }

        private void ConfigureIndexes(EntityTypeBuilder<BusRoutes> builder)
        {
            builder.HasIndex(br => br.RouteName);
        }
    }
}
