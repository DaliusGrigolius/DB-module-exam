using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
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
                    .Where(i => i.Id == clientId)
                    .SingleOrDefault();
                client.RestaurantId = moveIntoRestaurantId;
                Rdbc.Clients.Update(client);
                client.Waiters.ForEach(i => i.RestaurantId = moveIntoRestaurantId);
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
    }
}
