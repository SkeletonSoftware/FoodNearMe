﻿using FoodNearMe.Services;
using System;
using Xamarin.Forms;
using FoodNearMe.Models;
using System.Threading.Tasks;
using CoreLocation;

[assembly: Dependency(typeof(FoodNearMe.iOS.Services.Location))]
namespace FoodNearMe.iOS.Services
{
    public class Location : ILocation
    {
        public async Task<Gps> GetLocation()
        {
            var locationManager = new CLLocationManager();
            if (locationManager.Location == null)
            {
                var source = new TaskCompletionSource<CLLocation>();
                EventHandler<CLLocationsUpdatedEventArgs> handler = (sender, e) =>
                {
                    source.TrySetResult(locationManager.Location);
                };

                locationManager.LocationsUpdated += handler;
                locationManager.RequestLocation();
                await source.Task;
                locationManager.LocationsUpdated -= handler;
            }
            return new Gps()
            {
                Latitude = locationManager.Location.Coordinate.Latitude,
                Longitude = locationManager.Location.Coordinate.Longitude,
            };
        }

        public async Task<bool> RequestPermissions()
        {
            var locationManager = new CLLocationManager();
            locationManager.RequestWhenInUseAuthorization();
            return true;
        }
    }
}
