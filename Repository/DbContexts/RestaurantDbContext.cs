using Microsoft.EntityFrameworkCore;
using Repository.DbConfigs;
using Repository.Entities;

namespace Repository.DbContexts
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Waiter> Waiters { get; set; }

        public RestaurantDbContext(IDbConfigurations options) : base(options.Options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasOne(i => i.Restaurant)
                .WithMany(i => i.Clients)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
