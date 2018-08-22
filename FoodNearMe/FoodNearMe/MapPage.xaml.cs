using FoodNearMe.Managers;
using FoodNearMe.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System;
using System.Threading.Tasks;
using FoodNearMe.DependencyServices;

namespace FoodNearMe
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var locationManager = DependencyService.Get<ILocation>();
            Gps location = null;

            if (await locationManager.RequestPermissions())
            {
                location = await locationManager.GetLocation();
                this.map.IsShowingUser = true;
            }

            if (location != null)
            {
                this.map.MoveToRegion(MapSpan.FromCenterAndRadius((Position)location, new Distance(RestaurantManager.SearchRadius)));
                var restaurants = await this.LoadRestaurants(location);
                this.RefreshMapPins(restaurants);
            }
            else
            {
                await DisplayAlert("Poloha", "Aplikace se nepodařilo získat vaší polohu", "OK");
            }
        }

        private async Task<List<Restaurant>> LoadRestaurants(Gps location)
        {
            try
            {
                var manager = new RestaurantManager();
                var restaurants = await manager.GetRestaurants(location);
                return restaurants;
            }
            catch (Exception)
            {
                await DisplayAlert("Restaurace", "Restaurace se nepodařilo načíst", "OK");
                return null;
            }
        }

        private void RefreshMapPins(List<Restaurant> restaurants)
        {
            map.CustomPins.Clear();
            if (restaurants?.Count > 0)
            {
                foreach (var item in restaurants)
                {
                    var pin = new CustomPin()
                    {
                        Color = this.GetColorForType(item.Type),
                        Description = item.Description,
                        Location = item.Location,
                        Title = item.DisplayName,
                    };
                    map.CustomPins.Add(pin);
                }
            }
        }

        private Color GetColorForType(RestaurantQuality type)
        {
            switch (type)
            {
                case RestaurantQuality.Bad:
                    return Color.DarkRed;
                case RestaurantQuality.Good:
                    return Color.Red;
                case RestaurantQuality.VeryGood:
                    return Color.Yellow;
                case RestaurantQuality.Amazing:
                    return Color.Green;
                default:
                    return Color.Gray;
            }
        }
    }
}
