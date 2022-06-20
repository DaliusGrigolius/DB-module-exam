using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.DbContexts
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> RestaurantsDb { get; set; }
        public DbSet<Client> ClientsDb { get; set; }
        public DbSet<Waiter> WaitersDb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=localhost;Database=RestaurantDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasOne(i => i.Restaurant)
                .WithMany(i => i.Clients)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
