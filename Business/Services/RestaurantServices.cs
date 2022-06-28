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
        private RestaurantDbContext Rdbc { get; }

        public RestaurantServices(RestaurantDbContext rdbc)
        {
            Rdbc = rdbc;
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
                Rdbc.Add(newRestaurant);

                Rdbc.SaveChanges();

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
                Rdbc.Add(new Restaurant(restaurantName, restaurantAddress, restaurantEmail, restaurantPhone));
                Rdbc.SaveChanges();

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
                var restaurant = Rdbc.Restaurants
                    .Include(i => i.Waiters)
                    .ThenInclude(i => i.Clients)
                    .Include(i => i.Clients)
                    .ThenInclude(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == restaurantId);
                if (restaurant == null)
                {
                    return new Result(false, "Error: Restaurant not found.");
                }

                Rdbc.Remove(restaurant);
                Rdbc.SaveChanges();

                return new Result(true, "Success: Restaurant deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public List<Client> ShowAllSpecificRestaurantClients(Guid restaurantId)
        {
            return Rdbc.Clients.Where(i => i.RestaurantId == restaurantId).ToList();
        }

        public List<Waiter> ShowAllSpecificRestaurantWaiters(Guid restaurantId)
        {
            return Rdbc.Waiters.Where(i => i.RestaurantId == restaurantId).ToList();
        }

        private List<Client> CreateClients(int clientsNumber, Guid restaurantId)
        {
            List<Client> list = new();
            for (int i = 0; i < clientsNumber; i++)
            {
                list.Add(new Client($"FirstName{i}", $"LastName{i}", restaurantId));
            }
            return list;
        }

        private List<Waiter> CreateWaiters(int waitersNumber)
        {
            List<Waiter> list = new();
            for (int i = 0; i < waitersNumber; i++)
            {
                list.Add(new Waiter($"FirstName{i}", $"LastName{i}", "Male", 18 + i));
            }
            return list;
        }

        private void AssignClientsToWaiters(List<Client> clients, List<Waiter> waiters)
        {
            for (int i = 0; i < waiters.Count; i++)
            {
                waiters[i].Clients.AddRange(clients);
            }
        }
    }
}
