using FoodNearMe.DataAccess.WebService;
using FoodNearMe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodNearMe.Managers
{
    class RestaurantManager
    {
        public const int SearchRadius = 2000;
        private const string GoogleApiKey = "replace this with your api key";

        public async Task<List<Restaurant>> GetRestaurants(Gps location)
        {
            var webRepository = new RestaurantsWebRepository();
            var output = await webRepository.GetRestaurants(location, SearchRadius, GoogleApiKey);
            return output;
        }
    }
}
