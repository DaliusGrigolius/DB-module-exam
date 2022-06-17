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
        public void CreateFullRestaurant(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone, int waitersNumber, int clientsNumber)
        {
            RestaurantDbContext rdbc = new RestaurantDbContext();

            List<Client> clients = new List<Client>();
            for (int i = 0; i < clientsNumber; i++)
            {
                clients.Add(new Client($"FirstName{i}", $"LastName{i}"));
            }

            List<Waiter> waiters = new List<Waiter>();
            for (int i = 0; i < waitersNumber; i++)
            {
                waiters.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 18 + i));
            }

            for (int i = 0; i < waiters.Count; i++)
            {
                waiters[i].Clients.AddRange(clients);
            }

            rdbc.Restaurants.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone, clients, waiters));
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
