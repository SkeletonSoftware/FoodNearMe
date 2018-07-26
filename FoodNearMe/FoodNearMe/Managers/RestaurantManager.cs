﻿using FoodNearMe.DataAccess.WebService;
using FoodNearMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodNearMe.Managers
{
    class RestaurantManager
    {
        public const int SearchRadius = 2000;

        public async Task<List<Restaurant>> GetRestaurants(Gps location)
        {
            var webRepository = new RestaurantsWebRepository();
            var output = await webRepository.GetRestaurants(location, SearchRadius);
            return output;
        }
    }
}
