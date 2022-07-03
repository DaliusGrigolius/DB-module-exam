using Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class ClientServices : IClientServices
    {
        private RestaurantDbContext _context { get; }

        public ClientServices(RestaurantDbContext context)
        {
            _context = context;
        }

        public Result AddNewClientToSpecificRestaurant(Guid restaurantID, string clientFirstName, string clientLastName)
        {
            try
            {
                var rest = _context.Restaurants.Find(restaurantID);
                if (rest == null)
                {
                    return new Result(false, "Error: Restaurant not found.");
                }

                var waiters = _context.Waiters.Where(i => i.RestaurantId == restaurantID).ToList();
                var newClient = new Client(clientFirstName, clientLastName, restaurantID);
                _context.Clients.Add(newClient);

                foreach (var waiter in waiters)
                {
                    waiter.Clients.Add(newClient);
                }

                _context.SaveChanges();

                return new Result(true, "Success: New client added.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result TransferTheClientToAnotherRestaurant(Guid clientId, Guid moveIntoRestaurantId)
        {
            try
            {
                var client = _context.Clients
                    .Include(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == clientId);
                if (client == null)
                {
                    return new Result(false, "Error: Client not found.");
                }

                client.Waiters.Clear();
                client.RestaurantId = moveIntoRestaurantId;

                var restaurant = _context.Restaurants
                    .Include(i => i.Waiters)
                    .FirstOrDefault(i => i.Id == moveIntoRestaurantId);
                if (restaurant == null)
                {
                    return new Result(false, "Error: Restaurant not found.");
                }

                client.Waiters.AddRange(restaurant.Waiters);
                _context.Clients.Update(client);
                _context.SaveChanges();

                return new Result(true, "Success: Client transfered.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result DeleteTheClient(Guid clientId)
        {
            try
            {
                var client = _context.Clients.Find(clientId);
                if (client == null)
                {
                    return new Result(false, "Error: Client not found.");
                }

                _context.Clients.Remove(client);
                _context.SaveChanges();

                return new Result(true, "Success: Client deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public Result DeleteTheClientByName(string firstName)
        {
            try
            {
                var client = _context.Clients.Find(firstName);
                if (client == null)
                {
                    return new Result(false, "Error: Client not found.");
                }

                _context.Clients.Remove(client);
                _context.SaveChanges();

                return new Result(true, "Success: Client deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public List<Client> ShowAllClientsBySpecificWaiter(Guid waiterId)
        {
            try
            {
                var waiter = _context.Waiters.Find(waiterId);
                if (waiter == null)
                {
                    return null;
                }
                return _context.Clients.Where(i => i.RestaurantId == waiter.RestaurantId).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Result AddNewDummyListOfClientsToSpecificRestaurant(Guid restaurantId, int clientsNumber)
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

                var clients = new List<Client>();
                for (int i = 0; i < clientsNumber; i++)
                {
                    clients.Add(new Client($"FirstName{i}", $"LastName{i}", restaurantId));
                }

                clients.ForEach(i => i.Waiters.AddRange(restaurant.Waiters));
                _context.Clients.AddRange(clients);
                restaurant.Waiters.ForEach(i => i.Clients.AddRange(clients));

                _context.SaveChanges();

                return new Result(true, $"Success: Clients added.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e}");
            }
        }
    }
}
