using Business;
using Business.Services;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository.DbConfigs;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTests.BusinessTests
{
    
    public class RestaurantServiceTests : IDisposable
    {
        private RestaurantServices RestaurantServices { get; }
        private RestaurantDbContext RestaurantDbContext { get; set; }
        private readonly Mock<IDbConfigurations> DbConfigurationsMock;

        public RestaurantServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("RestaurantTestDb").Options;
            DbConfigurationsMock = new Mock<IDbConfigurations>();
            DbConfigurationsMock.Setup(i => i.ConnectionString).Returns("RestaurantTestDb");
            DbConfigurationsMock.Setup(i => i.Options).Returns(dbContextOptions);
            RestaurantDbContext = new RestaurantDbContext(DbConfigurationsMock.Object);
            RestaurantServices = new RestaurantServices(RestaurantDbContext);
            RestaurantDbContext.Database.EnsureCreated();
        }

        [Fact]
        public void CreateEmptyRestaurant_AddsData_ReturnsTrue()
        {
            var actual = RestaurantServices.CreateEmptyRestaurant("a", "b", "c", "d");
            var expected = new Result(true, "Success: Restaurant created.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var restaurantsList = new List<Restaurant>();
            foreach (var rest in RestaurantDbContext.Restaurants)
            {
                restaurantsList.Add(rest);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restaurantsList.Count == 1);
        }

        [Fact]
        public void CreateRestaurantWithNewWaitersAndClients_AddsData_ReturnsTrue()
        {
            var actual = RestaurantServices.CreateRestaurantWithNewWaitersAndClients("a", "b", "c", "d", 5, 5);
            var expected = new Result(true, "Success: Restaurant created.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var restaurantsList = new List<Restaurant>();
            foreach (var restaurant in RestaurantDbContext.Restaurants)
            {
                restaurantsList.Add(restaurant);
            }
            var waitersList = new List<Waiter>();
            foreach (var waiter in RestaurantDbContext.Waiters)
            {
                waitersList.Add(waiter);
            }
            var clientsList = new List<Client>();
            foreach (var client in RestaurantDbContext.Clients)
            {
                clientsList.Add(client);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restaurantsList.Count == 1);
            Assert.True(waitersList.Count == 5);
            Assert.True(clientsList.Count == 5);
        }

        [Fact]
        public void DeleteRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = RestaurantServices.DeleteRestaurant(new Guid());
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void DeleteRestaurant_RemovesData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            RestaurantDbContext.Add(restaurant);
            RestaurantDbContext.SaveChanges();

            var actual = RestaurantServices.DeleteRestaurant(restaurant.Id);
            var expected = new Result(true, "Success: Restaurant deleted.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void ShowAllSpecificRestaurantClients_HasData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            RestaurantDbContext.Add(restaurant);
            var clientList = new List<Client>
            {
                new Client("a", "b", restaurant.Id),
                new Client("c", "d", restaurant.Id),
                new Client("e", "f", restaurant.Id),
            };
            RestaurantDbContext.AddRange(clientList);
            RestaurantDbContext.SaveChanges();

            var actual = RestaurantServices.ShowAllSpecificRestaurantClients(restaurant.Id);
            var expected = clientList;
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void ShowAllSpecificRestaurantWaiters_HasData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            var waitersList = new List<Waiter>
            {
                new Waiter("a", "b", "c", 20),
                new Waiter("a", "b", "c",18),
                new Waiter("a", "b", "c", 19),
            };
            restaurant.Waiters.AddRange(waitersList);
            RestaurantDbContext.Add(restaurant);
            RestaurantDbContext.SaveChanges();

            var actual = RestaurantServices.ShowAllSpecificRestaurantWaiters(restaurant.Id);
            var expected = waitersList;
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        public void Dispose()
        {
            RestaurantDbContext.Database.EnsureDeleted();
        }
    }
}
