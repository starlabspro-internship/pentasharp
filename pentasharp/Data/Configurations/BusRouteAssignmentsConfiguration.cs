using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pentasharp.Models.Entities;

namespace pentasharp.Data.Configurations
{
    public class BusRouteAssignmentsConfiguration : IEntityTypeConfiguration<BusRouteAssignments>
    {
        public void Configure(EntityTypeBuilder<BusRouteAssignments> builder)
        {
            ConfigureKeys(builder);           
            ConfigureProperties(builder);     
            ConfigureDefaults(builder);      
            ConfigureRelationships(builder);  
        }

        private void ConfigureKeys(EntityTypeBuilder<BusRouteAssignments> builder)
        {
            builder.HasKey(ba => ba.AssignmentId);
        }

        private void ConfigureProperties(EntityTypeBuilder<BusRouteAssignments> builder)
        {
            builder.Property(ba => ba.AssignmentId)
                .ValueGeneratedOnAdd();

            builder.Property(ba => ba.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(ba => ba.UpdatedAt)
                .IsRequired(false);
        }

        private void ConfigureDefaults(EntityTypeBuilder<BusRouteAssignments> builder)
        {
            builder.Property(ba => ba.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private void ConfigureRelationships(EntityTypeBuilder<BusRouteAssignments> builder)
        {
            builder.HasOne(ba => ba.Bus)
                .WithMany()
                .HasForeignKey(ba => ba.BusId);

            builder.HasOne(ba => ba.Route)
                .WithMany()
                .HasForeignKey(ba => ba.RouteId);
        }
    }
}
