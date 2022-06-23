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

        public Result TransferTheClientToAnotherRestaurant(Guid clientId, Guid moveIntoRestaurantId)
        {
            try
            {
                var client = Rdbc.Clients
                    .Include(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == clientId);

                client.RestaurantId = moveIntoRestaurantId;
                Rdbc.Clients.Update(client);
                client.Waiters.ForEach(i => i.RestaurantId = moveIntoRestaurantId);//----------
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
                var client = Rdbc.Clients
                    .Where(i => i.Id == clientId)
                    .SingleOrDefault();
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
            try
            {
                var waiter = Rdbc.Waiters.Find(waiterId);
                var clientsList = new List<Client>();
                foreach (var client in waiter.Clients)
                {
                    clientsList.Add(client);
                }
                return clientsList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Result AddNewDummyListOfClientsToSpecificRestaurant(Guid restaurantId, int clientsNumber)
        {
            try
            {
                var restaurant = Rdbc.Restaurants.Find(restaurantId);
                var clients = new List<Client>();
                for (int i = 0; i < clientsNumber; i++)
                {
                    clients.Add(new Client($"FirstName{i}", $"LastName{i}", restaurantId));
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
