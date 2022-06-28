using Business;
using Business.Services;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTests.BusinessTests
{
    
    public class RestaurantServiceTests : IDisposable
    {
        private RestaurantServices Rs { get; }
        private RestaurantDbContext Rdbc { get; set; }

        public RestaurantServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("RestaurantTestDb").Options;
            Rdbc = new RestaurantDbContext(dbContextOptions);
            Rdbc.Database.EnsureCreated();
            Rs = new RestaurantServices(Rdbc);
        }

        [Fact]
        public void CreateEmptyRestaurant_AddsData_ReturnsTrue()
        {
            var actual = Rs.CreateEmptyRestaurant("a", "b", "c", "d");
            var expected = new Result(true, "Success: Restaurant created.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var restList = new List<Restaurant>();
            foreach (var rest in Rdbc.Restaurants)
            {
                restList.Add(rest);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restList.Count == 1);
        }

        [Fact]
        public void CreateRestaurantWithNewWaitersAndClients_AddsData_ReturnsTrue()
        {
            var actual = Rs.CreateRestaurantWithNewWaitersAndClients("a", "b", "c", "d", 5, 5);
            var expected = new Result(true, "Success: Restaurant created.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var restList = new List<Restaurant>();
            foreach (var rest in Rdbc.Restaurants)
            {
                restList.Add(rest);
            }
            var waitersList = new List<Waiter>();
            foreach (var waiter in Rdbc.Waiters)
            {
                waitersList.Add(waiter);
            }
            var clientsList = new List<Client>();
            foreach (var client in Rdbc.Clients)
            {
                clientsList.Add(client);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restList.Count == 1);
            Assert.True(waitersList.Count == 5);
            Assert.True(clientsList.Count == 5);
        }

        [Fact]
        public void DeleteRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = Rs.DeleteRestaurant(new Guid());
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void DeleteRestaurant_RemovesData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            Rdbc.SaveChanges();

            var actual = Rs.DeleteRestaurant(rest.Id);
            var expected = new Result(true, "Success: Restaurant deleted.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void ShowAllSpecificRestaurantClients_HasData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            var clientList = new List<Client>
            {
                new Client("a", "b", rest.Id),
                new Client("c", "d", rest.Id),
                new Client("e", "f", rest.Id),
            };
            Rdbc.AddRange(clientList);
            Rdbc.SaveChanges();

            var actual = Rs.ShowAllSpecificRestaurantClients(rest.Id);
            var expected = clientList;
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void ShowAllSpecificRestaurantWaiters_HasData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            var waitersList = new List<Waiter>
            {
                new Waiter("a", "b", "c", 20),
                new Waiter("a", "b", "c",18),
                new Waiter("a", "b", "c", 19),
            };
            rest.Waiters.AddRange(waitersList);
            Rdbc.Add(rest);
            Rdbc.SaveChanges();

            var actual = Rs.ShowAllSpecificRestaurantWaiters(rest.Id);
            var expected = waitersList;
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        public void Dispose()
        {
            Rdbc.Database.EnsureDeleted();
        }
    }
}
