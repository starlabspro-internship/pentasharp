using Microsoft.EntityFrameworkCore;
using pentasharp.Models.Entities;
using pentasharp.Data.Configurations;

namespace pentasharp.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TaxiCompany> TaxiCompanies { get; set; }
        public DbSet<Taxi> Taxis { get; set; }
        public DbSet<TaxiReservations> TaxiReservations { get; set; }
        public DbSet<BusSchedule> BusSchedules { get; set; }
        public DbSet<BusRouteAssignments> BusRouteAssignments { get; set; }
        public DbSet<BusReservations> BusReservations { get; set; }
        public DbSet<BusCompany> BusCompanies { get; set; }
        public DbSet<Buses> Buses { get; set; }
        public DbSet<BusRoutes> BusRoutes { get; set; }
        public DbSet<TaxiBookings> TaxiBookings { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfiguration(new UserConfiguration());
           modelBuilder.ApplyConfiguration(new TaxiCompanyConfiguration());
           modelBuilder.ApplyConfiguration(new TaxiConfiguration());
           modelBuilder.ApplyConfiguration(new TaxiReservationsConfiguration());
           modelBuilder.ApplyConfiguration(new BusScheduleConfiguration());
           modelBuilder.ApplyConfiguration(new BusRouteAssignmentsConfiguration());
           modelBuilder.ApplyConfiguration(new BusReservationsConfiguration());
            modelBuilder.ApplyConfiguration(new TaxiBookingConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        }
    }
}