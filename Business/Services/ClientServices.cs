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
            Rdbc = new RestaurantDbContext();
        }

        public string AddNewClientToSpecificRestaurant(string restaurantID, string clientFirstName, string clientLastName)
        {
            bool parse = Guid.TryParse(restaurantID, out Guid restaurantIDParsed);
            if (!parse)
            {
                return "Error: parsing was unsuccessful.";
            }

            var rest = Rdbc.RestaurantsDb.Find(restaurantIDParsed);
            if (rest == null)
            {
                return "Error! Restaurant not found!";
            }

            var rest1 = Rdbc.WaitersDb.Where(i => i.RestaurantId == restaurantIDParsed).ToList();
            var newClient = new Client("client2", "client2", new Guid("C9C062E1-E803-4A2A-9248-EEB564253357"));
            Rdbc.ClientsDb.Add(newClient);
            //Rdbc.SaveChanges();
            foreach (var waiter in rest1)
            {
                waiter.Clients.Add(newClient);
            }

            Rdbc.SaveChanges();

            return "Success! New client added";
        }
    }
}
