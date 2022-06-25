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

        public Result AddNewWaiterToSpecificRestaurant(Guid restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge)
        {
            try
            {
                var restaurant = Rdbc.Restaurants.Find(restaurantID);
                if (restaurant == null)
                {
                    return new Result(false, "Error! Restaurant not found!");
                }

                var clients = Rdbc.Clients.Where(i => i.RestaurantId == restaurantID).ToList();
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
                    .FirstOrDefault(i => i.Id == waiterId);
                if (waiter == null)
                {
                    return new Result(false, "Error! Waiter not found!");
                }

                var restaurant = Rdbc.Restaurants
                    .Include(i => i.Clients)
                    .FirstOrDefault(i => i.Id == moveIntoRestaurantId);
                if (restaurant == null)
                {
                    return new Result(false, "Error! Restaurant not found!");
                }

                waiter.Clients.Clear();
                waiter.RestaurantId = moveIntoRestaurantId;


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

        public Result DeleteTheWaiter(Guid waiterId)
        {
            try
            {
                var waiter = Rdbc.Waiters.Find(waiterId);
                if (waiter == null)
                {
                    return new Result(false, "Error! Waiter not found!");
                }

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

                var waiters = new List<Waiter>();
                for (int i = 0; i < waitersNumber; i++)
                {
                    waiters.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 19 + i));
                }

                waiters.ForEach(i => i.RestaurantId = restaurantId);
                waiters.ForEach(i => i.Clients.AddRange(restaurant.Clients));
                Rdbc.Waiters.AddRange(waiters);
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
