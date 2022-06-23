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

        public Result TransferTheWaiterToAnotherRestaurant(Guid waiterId, Guid moveIntoRestaurantId)
        {
            try
            {
                var waiter = Rdbc.Waiters
                    .Include(i => i.Clients)
                    .Where(i => i.Id == waiterId)
                    .SingleOrDefault();
                waiter.RestaurantId = moveIntoRestaurantId;
                Rdbc.Waiters.Update(waiter);
                waiter.Clients.ForEach(i => i.RestaurantId = moveIntoRestaurantId); 
                Rdbc.SaveChanges();

                return new Result(true, "Success! Waiter transfered.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result DeleteTheWaiter(Guid waiterId)
        {
            try
            {
                var waiter = Rdbc.Waiters.Find(waiterId);
                Rdbc.Waiters.Remove(waiter);
                Rdbc.SaveChanges();

                return new Result(true, "Success! Waiter deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result AddNewDummyListOfWaitersToSpecificRestaurant(Guid restaurantId, int waitersNumber)
        {
            try
            {
                var restaurant = Rdbc.Restaurants.Find(restaurantId);
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
