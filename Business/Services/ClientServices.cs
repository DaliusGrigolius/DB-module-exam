using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class ClientServices
    {
        private RestaurantDbContext Rdbc { get; }

        public ClientServices()
        {
            Rdbc = new();
        }

        public Result AddNewClientToSpecificRestaurant(string restaurantID, string clientFirstName, string clientLastName)
        {
            try
            {
                bool parse = Guid.TryParse(restaurantID, out Guid restaurantIDParsed);
                if (!parse)
                {
                    return new Result(false, "Error! parsing unsuccessful.");
                }

                var rest = Rdbc.Restaurants.Find(restaurantIDParsed);
                if (rest == null)
                {
                    return new Result(false, "Error! Restaurant not found!");
                }

                var waiters = Rdbc.Waiters.Where(i => i.RestaurantId == restaurantIDParsed).ToList();
                var newClient = new Client(clientFirstName, clientLastName, restaurantIDParsed);
                Rdbc.Clients.Add(newClient);
                foreach (var waiter in waiters)
                {
                    waiter.Clients.Add(newClient);
                }

                Rdbc.SaveChanges();
               
                return new Result(true, "Success! New client added");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result TransferTheClientToAnotherRestaurant(string clientId, string moveIntoRestaurantId)
        {
            try
            {
                bool parse = Guid.TryParse(clientId, out Guid clientIDParsed);
                bool parse1 = Guid.TryParse(moveIntoRestaurantId, out Guid restaurnatIdParsed);

                var client = Rdbc.Clients
                    .Include(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == clientIDParsed);

                client.Waiters.Clear();
                client.RestaurantId = restaurnatIdParsed;

                var restaurant = Rdbc.Restaurants
                    .Include(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == restaurnatIdParsed);

                client.Waiters.AddRange(restaurant.Waiters);
                Rdbc.Clients.Update(client);
                Rdbc.SaveChanges();

                return new Result(true, "Success! Client transfered.");
            }
            catch(Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result DeleteTheClient(string clientId)
        {
            try
            {
                bool parse = Guid.TryParse(clientId, out Guid clientIDParsed);
                var client = Rdbc.Clients.Find(clientIDParsed);
                Rdbc.Clients.Remove(client);
                Rdbc.SaveChanges();

                return new Result(true, "Success! Client deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public List<Client> ShowAllClientsBySpecificWaiter(string waiterId)
        {
            try
            {
                bool parse = Guid.TryParse(waiterId, out Guid waiterIDParsed);
                var waiter = Rdbc.Waiters.Find(waiterIDParsed);
                var clients = Rdbc.Clients.Where(i => i.RestaurantId == waiter.RestaurantId);

                return clients.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Result AddNewDummyListOfClientsToSpecificRestaurant(string restaurantId, int clientsNumber)
        {
            try
            {
                bool parse = Guid.TryParse(restaurantId, out Guid restaurantIDParsed);
                var restaurant = Rdbc.Restaurants.Find(restaurantIDParsed);
                var clients = new List<Client>();
                for (int i = 0; i < clientsNumber; i++)
                {
                    clients.Add(new Client($"FirstName{i}", $"LastName{i}", restaurantIDParsed));
                }
                restaurant.Clients.AddRange(clients);
                restaurant.Waiters.ForEach(i => i.Clients.AddRange(clients));

                Rdbc.SaveChanges();

                return new Result(true, $"Success! Clients added.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e}");
            }
        }
    }
}
