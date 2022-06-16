using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RestaurantServices
    {
        public void CreateRestaurant()
        {
            RestaurantDbContext rdbc = new RestaurantDbContext();
            rdbc.Add(new Restaurant());
        }
    }
}
