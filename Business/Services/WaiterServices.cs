using Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class WaiterServices : IWaiterServices
    {
        private RestaurantDbContext _context { get; }

        public WaiterServices(RestaurantDbContext context)
        {
            _context = context;
        }

        public Result AddNewWaiterToSpecificRestaurant(Guid restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge)
        {
            try
            {
                var restaurant = _context.Restaurants.Find(restaurantID);
                if (restaurant == null)
                {
                    return new Result(false, "Error: Restaurant not found.");
                }

                var clients = _context.Clients.Where(i => i.RestaurantId == restaurantID).ToList();
                var newWaiter = new Waiter(waiterFirstName, waiterLastName, waiterGender, waiterAge);
                _context.Add(newWaiter);
                restaurant.Waiters.Add(newWaiter);

                foreach (var client in clients)
                {
                    client.Waiters.Add(newWaiter);
                }

                _context.SaveChanges();

                return new Result(true, "Success: New waiter added.");
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
                var waiter = _context.Waiters
                    .Include(i => i.Clients)
                    .FirstOrDefault(i => i.Id == waiterId);
                if (waiter == null)
                {
                    return new Result(false, "Error: Waiter not found.");
                }

                var restaurant = _context.Restaurants
                    .Include(i => i.Clients)
                    .FirstOrDefault(i => i.Id == moveIntoRestaurantId);
                if (restaurant == null)
                {
                    return new Result(false, "Error: Restaurant not found.");
                }

                waiter.Clients.Clear();
                waiter.RestaurantId = moveIntoRestaurantId;


                waiter.Clients.AddRange(restaurant.Clients);
                _context.Waiters.Update(waiter);
                _context.SaveChanges();

                return new Result(true, "Success: Waiter transfered.");
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
                var waiter = _context.Waiters.Find(waiterId);
                if (waiter == null)
                {
                    return new Result(false, "Error: Waiter not found.");
                }

                _context.Waiters.Remove(waiter);
                _context.SaveChanges();

                return new Result(true, "Success: Waiter deleted.");
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
                var restaurant = _context.Restaurants
                    .Include(i => i.Waiters)
                    .ThenInclude(i => i.Clients)
                    .Include(i => i.Clients)
                    .ThenInclude(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == restaurantId);
                if (restaurant == null)
                {
                    return new Result(false, "Error: Restaurant not found.");
                }

                var waiters = new List<Waiter>();
                for (int i = 0; i < waitersNumber; i++)
                {
                    waiters.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 19 + i));
                }

                waiters.ForEach(i => i.RestaurantId = restaurantId);
                waiters.ForEach(i => i.Clients.AddRange(restaurant.Clients));
                _context.Waiters.AddRange(waiters);
                restaurant.Clients.ForEach(i => i.Waiters.AddRange(waiters));

                _context.SaveChanges();

                return new Result(true, "Success: Waiters added.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e}");
            }
        }
    }
}
