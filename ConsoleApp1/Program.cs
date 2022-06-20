using Business.Services;
using Repository.DbContexts;
using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //ClientServices cs = new ClientServices();
            //Console.WriteLine(cs.AddNewClientToSpecificRestaurant("C9C062E1-E803-4A2A-9248-EEB564253357",  "Test5", "Test5"));

            RestaurantServices rs = new RestaurantServices();
            rs.CreateNewRestaurantOnlyWithExistingWaitersAndClients("R4", "address4", "email4", "phone4");

            //RestaurantDbContext rdbc = new RestaurantDbContext();
            //migrationBuilder.Sql($"update RestaurantDB.dbo.ClientsDb set RestaurantId = 'C9C062E1-E803-4A2A-9248-EEB564253357' where RestaurantId is null");
        }
    }
}
