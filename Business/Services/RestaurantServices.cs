using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RestaurantServices
    {
        public void CreateFullRestaurant(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone)
        {
            RestaurantDbContext rdbc = new RestaurantDbContext();

            List<Client> clients = new List<Client>
            {
                new Client("FirstName1", "LastName1"),
                new Client("FirstName2", "LastName2"),
                new Client("FirstName3", "LastName3"),
                new Client("FirstName4", "LastName4"),
                new Client("FirstName5", "LastName5"),
            };

            List<Waiter> waiters = new List<Waiter>
            {
                new Waiter("FirstName1", "LastName1", "Female", 18),
                new Waiter("FirstName1", "LastName1", "Male", 19),
                new Waiter("FirstName1", "LastName1", "Female", 22),
                new Waiter("FirstName1", "LastName1", "Female", 20),
                new Waiter("FirstName1", "LastName1", "Male", 21),
            };

            rdbc.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone, clients, waiters));
            rdbc.SaveChanges();
        }

        public void CreateEmptyRestaurant(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone)
        {
            RestaurantDbContext rdbc = new RestaurantDbContext();
            rdbc.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone));
            rdbc.SaveChanges();
        }
    }
}
