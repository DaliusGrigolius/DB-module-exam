using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.DbContexts
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Waiter> Waiters { get; set; }

        public RestaurantDbContext(DbContextOptions options) : base(options)
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
