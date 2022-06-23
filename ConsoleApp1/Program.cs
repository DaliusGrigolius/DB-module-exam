using Business.Services;

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

            rs.ShowAllSpecificRestaurantClients("6BD21732-90E1-4C22-BDE7-47837974C761");
            //Console.WriteLine(ws.AddNewWaiterToSpecificRestaurant("22856010-8878-4013-ABB3-5CA0C9F03B8A", "tesc", "tesc", "tesc", 20));
            //Console.WriteLine(cs.AddNewClientToSpecificRestaurant("57139EDF-9B96-407A-884B-77416B3304D8", "client1", "client1"));
            //var result = cs.AddNewClientToSpecificRestaurant("22856010-8878-4013-ABB3-5CA0C9F03B8A", "tesc", "tesc");
            //if (result.IsSuccess)
            //{
            //    Console.WriteLine(result.Message);
            //}
            //else
            //{
            //    Console.WriteLine("END OF LIFE!");
            //}

            //var result = ws.TransferTheWaiterToAnotherRestaurant(new Guid("5DF18EC5-8724-4052-BB10-69BC0E007414"), new Guid("57139EDF-9B96-407A-884B-77416B3304D8"));
            //if (result.IsSuccess)
            //{
            //    Console.WriteLine(result.Message);
            //}
            //else
            //{
            //    Console.WriteLine("END OF LIFE!");
            //}
            //var result = rs.DeleteRestaurant(new Guid("969A4903-D9B2-4156-968E-C559D0426630"));
            //if (result.IsSuccess)
            //{
            //    Console.WriteLine(result.Message);
            //}
            //else
            //{
            //    Console.WriteLine(result.Message);
            //}
            //Console.WriteLine(rs.CreateRestaurantWithNewWaitersAndClients("R2", "address2", "email2", "phone2", 2, 2));
            //Console.WriteLine(cs.TransferTheClientToAnotherRestaurant(new Guid("AAD70548-1F17-4F28-AF9B-30A94B68BA64"), new Guid("22856010-8878-4013-ABB3-5CA0C9F03B8A")));
            //Console.WriteLine(cs.TransferTheClientToAnotherRestaurant(new Guid("3B778653-197A-4BD5-8EC6-423476A0FF3A"), new Guid("5034C81F-014D-4D36-A844-70EDAF0F7C71")));
            //Console.WriteLine(cs.DeleteTheClient(new Guid("8F828582-8C92-4D6A-AAFA-AD0F10F856A9")));
            //Console.WriteLine(cs.DeleteTheClient(new Guid("4BF74786-5A57-441D-9003-B67B45B484FA")));
            //Console.WriteLine(ws.TransferTheWaiterToAnotherRestaurant(new Guid("17589615-E0FC-4585-9DAF-512C0529D9C3"), new Guid("5034C81F-014D-4D36-A844-70EDAF0F7C71")));

            //RestaurantDbContext rdbc = new RestaurantDbContext();
            //migrationBuilder.Sql($"update RestaurantDB.dbo.ClientsDb set RestaurantId = 'C9C062E1-E803-4A2A-9248-EEB564253357' where RestaurantId is null");
        }
    }
}
