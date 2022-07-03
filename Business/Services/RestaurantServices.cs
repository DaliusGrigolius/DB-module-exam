using Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class RestaurantServices : IRestaurantServices
    {
        private RestaurantDbContext _context { get; }

        public RestaurantServices(RestaurantDbContext context)
        {
            _context = context;
        }

        public Result CreateRestaurantWithNewWaitersAndClients(string name, string address, string email, string phone, int waitersNumber, int clientsNumber)
        {
            try
            {
                var newRestaurant = new Restaurant(name, address, email, phone);

                var newClients = CreateClients(clientsNumber, newRestaurant.Id);
                var newWaiters = CreateWaiters(waitersNumber);
                AssignClientsToWaiters(newClients, newWaiters);
                newRestaurant.Waiters.AddRange(newWaiters);
                newRestaurant.Clients.AddRange(newClients);
                _context.Add(newRestaurant);

                _context.SaveChanges();

                return new Result(true, "Success: Restaurant created.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e}");
            }
        }

        public Result CreateEmptyRestaurant(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone)
        {
            try
            {
                _context.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone));
                _context.SaveChanges();

                return new Result(true, "Success: Restaurant created.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e}");
            }
        }

        public Result DeleteRestaurant(Guid restaurantId)
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

                _context.Remove(restaurant);
                _context.SaveChanges();

                return new Result(true, "Success: Restaurant deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public List<Client> ShowAllSpecificRestaurantClients(Guid restaurantId)
        {
            try
            {
                return _context.Clients.Where(i => i.RestaurantId == restaurantId).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Waiter> ShowAllSpecificRestaurantWaiters(Guid restaurantId)
        {
            try
            {
                return _context.Waiters.Where(i => i.RestaurantId == restaurantId).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private List<Client> CreateClients(int clientsNumber, Guid restaurantId)
        {
            try
            {
                List<Client> list = new();
                for (int i = 0; i < clientsNumber; i++)
                {
                    list.Add(new Client($"FirstName{i}", $"LastName{i}", restaurantId));
                }
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private List<Waiter> CreateWaiters(int waitersNumber)
        {
            try
            {
                List<Waiter> list = new();
                for (int i = 0; i < waitersNumber; i++)
                {
                    list.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 18 + i));
                }
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void AssignClientsToWaiters(List<Client> clients, List<Waiter> waiters)
        {
            try
            {
                for (int i = 0; i < waiters.Count; i++)
                {
                    waiters[i].Clients.AddRange(clients);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
