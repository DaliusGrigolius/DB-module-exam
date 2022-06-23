using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
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

        public Result AddNewWaiterToSpecificRestaurant(string restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge)
        {
            try
            {
                bool parse = Guid.TryParse(restaurantID, out Guid restaurantIDParsed);
                if (!parse)
                {
                    return new Result(false, "Error: parsing was unsuccessful.");
                }

                var restaurant = Rdbc.Restaurants.Find(restaurantIDParsed);
                if (restaurant == null)
                {
                    return new Result(false, "Error! Restaurant not found!");
                }

                var clients = Rdbc.Clients.Where(i => i.RestaurantId == restaurantIDParsed).ToList();
                var newWaiter = new Waiter(waiterFirstName, waiterLastName, waiterGender, waiterAge);
                Rdbc.Add(newWaiter);
                restaurant.Waiters.Add(newWaiter);

                foreach (var client in clients)
                {
                    client.Waiters.Add(newWaiter);
                }

                Rdbc.SaveChanges();

                return new Result(true, "Success! New waiter added");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result TransferTheWaiterToAnotherRestaurant(string waiterId, string moveIntoRestaurantId)
        {
            try
            {
                bool parse = Guid.TryParse(waiterId, out Guid waiterIdParsed);
                bool parse1 = Guid.TryParse(moveIntoRestaurantId, out Guid restaurnatIdParsed);

                var waiter = Rdbc.Waiters
                    .Include(i => i.Clients)
                    .FirstOrDefault(i => i.Id == waiterIdParsed);

                waiter.Clients.Clear();
                waiter.RestaurantId = restaurnatIdParsed;

                var restaurant = Rdbc.Restaurants
                    .Include(i => i.Clients)
                    .FirstOrDefault(i => i.Id == restaurnatIdParsed);

                waiter.Clients.AddRange(restaurant.Clients);
                Rdbc.Waiters.Update(waiter);
                Rdbc.SaveChanges();

                return new Result(true, "Success! Waiter transfered.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result DeleteTheWaiter(string waiterId)
        {
            try
            {
                bool parse = Guid.TryParse(waiterId, out Guid waiterIDParsed);
                var waiter = Rdbc.Waiters.Find(waiterIDParsed);
                Rdbc.Waiters.Remove(waiter);
                Rdbc.SaveChanges();

                return new Result(true, "Success! Waiter deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result AddNewDummyListOfWaitersToSpecificRestaurant(string restaurantId, int waitersNumber)
        {
            try
            {
                bool parse = Guid.TryParse(restaurantId, out Guid restaurantIDParsed);
                var restaurant = Rdbc.Restaurants.Find(restaurantIDParsed);
                var waiters = new List<Waiter>();
                for (int i = 0; i < waitersNumber; i++)
                {
                    waiters.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 19 + i));
                }
                restaurant.Waiters.AddRange(waiters);
                restaurant.Clients.ForEach(i => i.Waiters.AddRange(waiters));

                Rdbc.SaveChanges();

                return new Result(true, $"Success! Waiters added.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e}");
            }
        }
    }
}
