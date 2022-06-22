using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Linq;

namespace Business.Services
{
    public class WaiterServices
    {
        private RestaurantDbContext Rdbc { get; }

        public WaiterServices()
        {
            Rdbc = new RestaurantDbContext();
        }

        public string AddNewWaiterToSpecificRestaurant(string restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge)
        {
            bool parse = Guid.TryParse(restaurantID, out Guid restaurantIDParsed);
            if (!parse)
            {
                return "Error: parsing was unsuccessful.";
            }

            var rest = Rdbc.Restaurants.Find(restaurantIDParsed);
            if (rest == null)
            {
                return "Error! Restaurant not found!";
            }

            var rest1 = Rdbc.Clients.Where(i => i.RestaurantId == restaurantIDParsed).ToList();
            var newWaiter = new Waiter(waiterFirstName, waiterLastName, waiterGender, waiterAge);
            Rdbc.Add(newWaiter);
            rest.Waiters.Add(newWaiter);

            foreach (var client in rest1)
            {
                client.Waiters.Add(newWaiter);
            }

            Rdbc.SaveChanges();

            return "Success! New waiter added";
        }
    }
}
