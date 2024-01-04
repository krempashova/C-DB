namespace Cadastre.Data
{
    using Cadastre.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Xml.Linq;

    public class CadastreContext : DbContext
    {
        public CadastreContext()
        {
            
        }

        public CadastreContext(DbContextOptions options)
            :base(options)
        {
            
        }
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<Citizen> Citizens { get; set; } = null!;
        public DbSet<District> Districts { get; set; } = null!;
        public DbSet<PropertyCitizen> PropertiesCitizens { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity <PropertyCitizen> (entity =>
            {
                entity.HasKey(pc => new { pc.PropertyId,pc.CitizenId});
            });

        }
    }
}
