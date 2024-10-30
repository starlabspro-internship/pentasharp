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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}