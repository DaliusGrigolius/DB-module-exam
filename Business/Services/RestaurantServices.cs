using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;

namespace Business.Services
{
    public class RestaurantServices
    {
        private RestaurantDbContext Rdbc { get; }

        public RestaurantServices()
        {
            Rdbc = new();           
        }

        public string CreateRestaurantWithNewWaitersAndClients(string name, string address, string email, string phone, int waitersNumber, int clientsNumber)
        {
            try
            {
                var newRest = new Restaurant(name, address, email, phone);

                var newClients = CreateClients(clientsNumber, newRest.Id);
                var newWaiters = CreateWaiters(waitersNumber);
                AssignClientsToWaiters(newClients, newWaiters);
                newRest.Waiters.AddRange(newWaiters);
                newRest.Clients.AddRange(newClients);
                Rdbc.Add(newRest);

                Rdbc.SaveChanges();

                return "Success! Restaurant created.";
            }
            catch (Exception e)
            {
                return $"Error: {e}";
            }
        }

        public string CreateEmptyRestaurant(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone)
        {
            Rdbc.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone));
            Rdbc.SaveChanges();

            return "Success! Restaurant created.";
        }

        private List<Client> CreateClients(int clientsNumber, Guid restId)
        {
            List<Client> list = new();
            for (int i = 0; i < clientsNumber; i++)
            {
                list.Add(new Client($"FirstName{i}", $"LastName{i}", restId));
            }
            return list;
        }

        private List<Waiter> CreateWaiters(int waitersNumber)
        {
            List<Waiter> list = new();
            for (int i = 0; i < waitersNumber; i++)
            {
               list.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 18 + i));
            }
            return list;
        }

        private void AssignClientsToWaiters(List<Client> clients, List<Waiter> waiters)
        {
            for (int i = 0; i < waiters.Count; i++)
            {
                waiters[i].Clients.AddRange(clients);
            }
        }
    }
}
