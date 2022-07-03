using Microsoft.EntityFrameworkCore;
using Repository.DbConfigs;
using Repository.Entities;

namespace Repository.DbContexts
{
    public class RestaurantDbContext : DbContext
    {
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Waiter> Waiters { get; set; }

        public RestaurantDbContext(IDbConfigurations options) : base(options.Options)
        {

        }

        public RestaurantDbContext()
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
