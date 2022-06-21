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
        public string AddNewClientToSpecificRestaurant(string restaurantID, string clientFirstName, string clientLastName)
        {
            RestaurantDbContext rdbc = new();

            bool parse = Guid.TryParse(restaurantID, out Guid restaurantIDParsed);
            if (!parse)
            {
                return "Error: parsing was unsuccessful.";
            }

            var rest = rdbc.Restaurants.Find(restaurantIDParsed);
            if (rest == null)
            {
                return "Error! Restaurant not found!";
            }

            var rest1 = rdbc.Waiters.Where(i => i.RestaurantId == restaurantIDParsed).ToList();
            var newClient = new Client(clientFirstName, clientLastName, restaurantIDParsed);
            rdbc.Clients.Add(newClient);
            foreach (var waiter in rest1)
            {
                waiter.Clients.Add(newClient);
            }

            rdbc.SaveChanges();

            return "Success! New client added";
        }

        public string TransferTheClientToAnotherRestaurant(Guid clientId, Guid moveIntoRestaurantId)
        {
            RestaurantDbContext rdbc = new();

            try
            {
                var client = rdbc.Clients
                    .Where(i => i.Id == clientId)
                    //.AsTracking()
                    .SingleOrDefault();

                client.RestaurantId = moveIntoRestaurantId;
                rdbc.Clients.Update(client);
                rdbc.SaveChanges();

                return "Success! Client transfered.";
            }
            catch(Exception e)
            {
                return $"Error: {e}";
            }
        }

        public string DeleteTheClient(Guid clientId)
        {
            RestaurantDbContext rdbc = new();

            var client = rdbc.Clients
                    .Where(i => i.Id == clientId)
                    //.AsTracking()
                    .SingleOrDefault();
            rdbc.Clients.Remove(client);
            rdbc.SaveChanges();

            return "Success! Client deleted.";
        }
    }
}
