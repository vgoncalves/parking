#nullable disable

using App.API.Data.TypeConfig;
using Microsoft.EntityFrameworkCore;

namespace App.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions opt)
            :base(opt) { }

        public DbSet<Parking> Parkings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParkingConfig).Assembly);
        }
    }
}
