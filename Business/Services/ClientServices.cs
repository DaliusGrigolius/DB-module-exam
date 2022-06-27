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

        public ClientServices(RestaurantDbContext rdbc)
        {
            Rdbc = rdbc;
        }

        public Result AddNewClientToSpecificRestaurant(Guid restaurantID, string clientFirstName, string clientLastName)
        {
            try
            {
                var rest = Rdbc.Restaurants.Find(restaurantID);
                if (rest == null)
                {
                    return new Result(false, "Error! Restaurant not found!");
                }

                var waiters = Rdbc.Waiters.Where(i => i.RestaurantId == restaurantID).ToList();
                var newClient = new Client(clientFirstName, clientLastName, restaurantID);
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

        public Result TransferTheClientToAnotherRestaurant(Guid clientId, Guid moveIntoRestaurantId)
        {
            try
            {
                var client = Rdbc.Clients
                    .Include(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == clientId);
                if (client == null)
                {
                    return new Result(false, "Error! Client not found!");
                }

                client.Waiters.Clear();
                client.RestaurantId = moveIntoRestaurantId;

                var restaurant = Rdbc.Restaurants
                    .Include(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == moveIntoRestaurantId);
                if (restaurant == null)
                {
                    return new Result(false, "Error! Restaurant not found!");
                }

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

        public Result DeleteTheClient(Guid clientId)
        {
            try
            {
                var client = Rdbc.Clients.Find(clientId);
                if (client == null)
                {
                    return new Result(false, "Error! Client not found!");
                }

                Rdbc.Clients.Remove(client);
                Rdbc.SaveChanges();

                return new Result(true, "Success! Client deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public List<Client> ShowAllClientsBySpecificWaiter(Guid waiterId)
        {
            var waiter = Rdbc.Waiters.Find(waiterId);
            var clients = Rdbc.Clients.Where(i => i.RestaurantId == waiter.RestaurantId);

            return clients.ToList();
        }

        public Result AddNewDummyListOfClientsToSpecificRestaurant(Guid restaurantId, int clientsNumber)
        {
            try
            {
                var restaurant = Rdbc.Restaurants
                    .Include(i => i.Waiters)
                    .ThenInclude(i => i.Clients)
                    .Include(i => i.Clients)
                    .ThenInclude(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == restaurantId);
                if (restaurant == null)
                {
                    return new Result(false, "Error! Restaurant not found!");
                }

                var clients = new List<Client>();
                for (int i = 0; i < clientsNumber; i++)
                {
                    clients.Add(new Client($"FirstName{i}", $"LastName{i}", restaurantId));
                }

                clients.ForEach(i => i.Waiters.AddRange(restaurant.Waiters));
                Rdbc.Clients.AddRange(clients);
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
