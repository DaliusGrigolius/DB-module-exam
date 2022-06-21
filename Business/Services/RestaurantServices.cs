using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class RestaurantServices
    {
        public string CreateRestaurantWithNewWaitersAndClients(string name, string address, string email, string phone, int waitersNumber, int clientsNumber)
        {
            RestaurantDbContext rdbc = new();
            rdbc.Add(new Restaurant(name, address, email, phone));
            rdbc.SaveChanges();

            var restByPhoneNumber = rdbc.Restaurants.Where(i => i.PhoneNumber == phone).SingleOrDefault();

            CreateClients(clientsNumber, restByPhoneNumber.Id, restByPhoneNumber);
            CreateWaiters(waitersNumber, restByPhoneNumber.Id, restByPhoneNumber);

            return "Success! Restaurant created.";
        }

        public string CreateEmptyRestaurant(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone)
        {
            RestaurantDbContext rdbc = new();
            rdbc.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone));
            rdbc.SaveChanges();

            return "Success! Restaurant created.";
        }

        private void CreateClients(int clientsNumber, Guid restId, Restaurant restaurant)
        {
            RestaurantDbContext rdbc = new();
            for (int i = 0; i < clientsNumber; i++)
            {
                restaurant.Clients.Add(new Client($"FirstName{i}", $"LastName{i}", restId));
            }
            rdbc.SaveChanges();
        }

        private void CreateWaiters(int waitersNumber, Guid restId, Restaurant restaurant)
        {
            RestaurantDbContext rdbc = new();
            for (int i = 0; i < waitersNumber; i++)
            {
                restaurant.Waiters.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 18 + i, restId));
            }
            rdbc.SaveChanges();
        }

        //private void AssignClientsToWaiters(List<Client> clients, List<Waiter> waiters)
        //{
        //    for (int i = 0; i < waiters.Count; i++)
        //    {
        //        waiters[i].Clients.AddRange(clients);
        //    }
        //}
    }
}
