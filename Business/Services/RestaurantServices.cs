using Repository.DbContexts;
using Repository.Entities;
using System.Collections.Generic;

namespace Business.Services
{
    public class RestaurantServices
    {
        private RestaurantDbContext rdbc { get; }
        private List<Client> existingClients { get; }
        private List<Waiter> existingWaiters { get; }

        public RestaurantServices()
        {
            rdbc = new RestaurantDbContext();
            existingClients = GetExistingClients();
            existingWaiters = GetExistingWaiters();
        }

        public void CreateNewRestaurantWithNewAndExistingWaitersAndClients(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone, int waitersNumber, int clientsNumber)
        {
            var clients = CreateClients(clientsNumber, existingClients);
            var waiters = CreateWaiters(waitersNumber, existingWaiters);
            AssignClientsToWaiters(clients, waiters);
            rdbc.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone, clients, waiters));
            rdbc.SaveChanges();
        }

        public void CreateNewRestaurantOnlyWithExistingWaitersAndClients(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone)
        {
            rdbc.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone, existingClients, existingWaiters));
            rdbc.SaveChanges();
        }

        private List<Client> CreateClients(int clientsNumber, List<Client> clients)
        {
            for (int i = 0; i < clientsNumber; i++)
            {
                clients.Add(new Client($"FirstName{i}", $"LastName{i}"));
            }
            return clients;
        }

        private List<Waiter> CreateWaiters(int waitersNumber, List<Waiter> waiters)
        {
            for (int i = 0; i < waitersNumber; i++)
            {
                waiters.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 18 + i));
            }
            return waiters;
        }

        private void AssignClientsToWaiters(List<Client> clients, List<Waiter> waiters)
        {
            for (int i = 0; i < waiters.Count; i++)
            {
                waiters[i].Clients.AddRange(clients);
            }
        }

        private List<Client> GetExistingClients()
        {
            var list = new List<Client>();
            foreach (var client in rdbc.Clients)
            {
                list.Add(client);
            }
            return list;
        }

        private List<Waiter> GetExistingWaiters()
        {
            var list = new List<Waiter>();
            foreach (var client in rdbc.Waiters)
            {
                list.Add(client);
            }
            return list;
        }
    }
}
