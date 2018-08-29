using FoodNearMe.DataAccess.WebService.Models;
using FoodNearMe.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodNearMe.DataAccess.WebService
{
    class RestaurantsWebRepository
    {
        public async Task<List<Restaurant>> GetRestaurants(Gps location, int radiusInMeters, string googleApiKey)
        {
            var client = new HttpClient();
            string url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={location}&radius={radiusInMeters}&type=restaurant&key={googleApiKey}";
            var response = await client.GetAsync(url);
            string data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RestaurantOutput>(data);
            return this.LoadRestaurants(result);
        }

        private List<Restaurant> LoadRestaurants(RestaurantOutput result)
        {
            var output = new List<Restaurant>(result.results.Length);

            foreach (var restaurant in result.results)
            {
                output.Add(new Restaurant()
                {
                    DisplayName = restaurant.name,
                    Location = new Gps()
                    {
                        Latitude = restaurant.geometry.location.lat,
                        Longitude = restaurant.geometry.location.lng,
                    },
                    Quality = LoadRestaurantQuality(restaurant.rating)
                });
            }

            return output;
        }

        private RestaurantQuality LoadRestaurantQuality(float rating)
        {
            if (rating > 4.5)
                return RestaurantQuality.Amazing;
            else if (rating > 4)
                return RestaurantQuality.VeryGood;
            else if (rating > 3)
                return RestaurantQuality.Good;
            else
                return RestaurantQuality.Bad;
        }
    }
}
