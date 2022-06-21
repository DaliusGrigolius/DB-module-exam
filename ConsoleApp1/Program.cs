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

            ClientServices cs = new ClientServices();
            WaiterServices ws = new WaiterServices();
            RestaurantServices rs = new RestaurantServices();

            //Console.WriteLine(ws.AddNewWaiterToSpecificRestaurant("3AB02D73-19E1-422A-A47D-B86AA938A342", "waiter2", "waiter2", "male", 19));
            //Console.WriteLine(cs.AddNewClientToSpecificRestaurant("5034C81F-014D-4D36-A844-70EDAF0F7C71", "client3", "client3"));
            //Console.WriteLine(cs.AddNewClientToSpecificRestaurant("3AB02D73-19E1-422A-A47D-B86AA938A342", "client4", "client4"));

            Console.WriteLine(rs.CreateRestaurantWithNewWaitersAndClients("R7", "address6", "email6", "phone8", 5, 5));
            //Console.WriteLine(cs.TransferTheClientToAnotherRestaurant(new Guid("4BF74786-5A57-441D-9003-B67B45B484FA"), new Guid("5034C81F-014D-4D36-A844-70EDAF0F7C71")));
            //Console.WriteLine(cs.TransferTheClientToAnotherRestaurant(new Guid("3B778653-197A-4BD5-8EC6-423476A0FF3A"), new Guid("5034C81F-014D-4D36-A844-70EDAF0F7C71")));
            //Console.WriteLine(cs.DeleteTheClient(new Guid("3B778653-197A-4BD5-8EC6-423476A0FF3A")));
            //Console.WriteLine(cs.DeleteTheClient(new Guid("4BF74786-5A57-441D-9003-B67B45B484FA")));
            //Console.WriteLine(ws.TransferTheWaiterToAnotherRestaurant(new Guid("17589615-E0FC-4585-9DAF-512C0529D9C3"), new Guid("5034C81F-014D-4D36-A844-70EDAF0F7C71")));

            //RestaurantDbContext rdbc = new RestaurantDbContext();
            //migrationBuilder.Sql($"update RestaurantDB.dbo.ClientsDb set RestaurantId = 'C9C062E1-E803-4A2A-9248-EEB564253357' where RestaurantId is null");
        }
    }
}
