using System;
using Xamarin.Forms;
using FoodNearMe.Models;
using System.Threading.Tasks;
using CoreLocation;
using FoodNearMe.DependencyServices;

[assembly: Dependency(typeof(FoodNearMe.iOS.DependencyServices.Location))]
namespace FoodNearMe.iOS.DependencyServices
{
    public class Location : ILocation
    {
        private CLLocationManager locationManager = new CLLocationManager();

        public async Task<Gps> GetLocation()
        {
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
            if (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways || CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                return true;
            }
            if (CLLocationManager.Status == CLAuthorizationStatus.Denied || CLLocationManager.Status == CLAuthorizationStatus.Restricted)
            {
                return false;
            }

            var taskCompletionSource = new TaskCompletionSource<bool>();

            locationManager.AuthorizationChanged += (sender, args) =>
            {
                if (args.Status != CLAuthorizationStatus.NotDetermined) //událost se poprvé vždy volá s tímto příznakem, chceme ale počkat až na reakci uživatele
                {
                    switch (args.Status)
                    {
                        case CLAuthorizationStatus.AuthorizedAlways:
                        case CLAuthorizationStatus.AuthorizedWhenInUse:
                            taskCompletionSource.TrySetResult(true);
                            break;
                        case CLAuthorizationStatus.Denied:
                        case CLAuthorizationStatus.Restricted:
                            taskCompletionSource.TrySetResult(false);
                            break;
                        default:
                            taskCompletionSource.TrySetResult(false);
                            break;
                    }
                }
            };

            locationManager.RequestWhenInUseAuthorization();

            return await taskCompletionSource.Task;
        }
    }
}