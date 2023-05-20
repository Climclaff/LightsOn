using LightOn.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LightOn.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Region>? Regions { get; set; }

        public DbSet<District>? Districts { get; set; }

        public DbSet<Town>? Towns { get; set; }

        public DbSet<Street>? Streets { get; set; }

        public DbSet<Transformer>? Transformers { get; set; }

        public DbSet<TransformerMeasurement>? TransformerMeasurements { get; set; }

        public DbSet<Building>? Buildings { get; set; }

        public DbSet<Review>? Reviews { get; set; }

        public DbSet<Appliance>? Appliances { get; set; }

        public DbSet<ApplianceUsageHistory>? ApplianceUsageHistories { get; set; }

        public DbSet<ApplianceUsagePlanned>? ApplianceUsagePlanneds { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("LightsOnDb");
            optionsBuilder.UseSqlServer(connectionString);
        }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
    }
}
