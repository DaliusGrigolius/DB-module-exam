using Repository.DbContexts;
using Repository.Entities;
using System;

namespace Business.Services
{
    public class ClientServices
    {
        private RestaurantDbContext rdbc { get; }

        public ClientServices()
        {
            rdbc = new RestaurantDbContext();
        }

        public string AddNewClient(Guid restaurantID, string clientFirstName, string clientLastName)
        {
            var rest = rdbc.Restaurants.Find(restaurantID);
            if (rest == null)
            {
                return "Error! Restaurant not found!";
            }
            rest.Clients.Add(new Client(clientFirstName, clientLastName));

            return "Success! New client added";
        }
    }
}
