using Business;
using Business.Services;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTests.BusinessClientServicesTests
{
    public class ClientServiceTests : IDisposable
    {
        private ClientServices Cs { get; }
        private RestaurantDbContext Rdbc { get; set; }

        public ClientServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("RestaurantTestDb").Options;
            Rdbc = new RestaurantDbContext(dbContextOptions);
            Rdbc.Database.EnsureCreated();
            Cs = new ClientServices(Rdbc);
        }

        [Fact]
        public void AddNewClientToSpecificRestaurant_RestaurantDoesntExist_ReturnFalse()
        {
            var actual = Cs.AddNewClientToSpecificRestaurant(new Guid(), "Daumantas", "Pavarde");
            var expected = new Result(false, "Error! Restaurant not found!");
            var comparisonResult = new CompareLogic().Compare(expected, actual);
            Assert.True(comparisonResult.AreEqual);
        }

        public void Dispose()
        {
            Rdbc.Database.EnsureDeleted();
        }
    }
}
