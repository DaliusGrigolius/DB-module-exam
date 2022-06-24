﻿using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class RestaurantServices
    {
        private RestaurantDbContext Rdbc { get; }

        public RestaurantServices()
        {
            Rdbc = new();           
        }

        public Result CreateRestaurantWithNewWaitersAndClients(string name, string address, string email, string phone, int waitersNumber, int clientsNumber)
        {
            try
            {
                var newRest = new Restaurant(name, address, email, phone);

                var newClients = CreateClients(clientsNumber, newRest.Id);
                var newWaiters = CreateWaiters(waitersNumber);
                AssignClientsToWaiters(newClients, newWaiters);
                newRest.Waiters.AddRange(newWaiters);
                newRest.Clients.AddRange(newClients);
                Rdbc.Add(newRest);

                Rdbc.SaveChanges();

                return new Result(true, "Success! Restaurant created.");
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

                return new Result(true, "Success! Restaurant created.");
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

                Rdbc.Remove(restaurant);
                Rdbc.SaveChanges();

                return new Result(true, "Success! Restaurant deleted.");
            }
            catch (Exception e)
            {
                return new Result(false, $"Error: {e.Message}");
            }
        }

        public List<Client> ShowAllSpecificRestaurantClients(Guid restaurantId)
        {
            var clients = Rdbc.Clients.Where(i => i.RestaurantId == restaurantId).ToList();
            return clients;
        }

        public List<Waiter> ShowAllSpecificRestaurantWaiters(Guid restaurantId)
        {
            var waiters = Rdbc.Waiters.Where(i => i.RestaurantId == restaurantId).ToList();
            return waiters;
        }

        private List<Client> CreateClients(int clientsNumber, Guid restId)
        {
            List<Client> list = new();
            for (int i = 0; i < clientsNumber; i++)
            {
                list.Add(new Client($"FirstName{i}", $"LastName{i}", restId));
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
