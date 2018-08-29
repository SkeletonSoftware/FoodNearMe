using System.Threading.Tasks;
using FoodNearMe.Models;
using FoodNearMe.DependencyServices;
using Android.Gms.Location;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(FoodNearMe.Droid.DependencyServices.Location))]
namespace FoodNearMe.Droid.DependencyServices
{
    public class Location : ILocation
    {
        public async Task<Gps> GetLocation()
        {
            var fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(CrossCurrentActivity.Current.Activity);

            Android.Locations.Location location = await fusedLocationProviderClient.GetLastLocationAsync();

            Gps output = new Gps();
            output.Latitude = location.Latitude;
            output.Longitude = location.Longitude;

            return output;
        }

        public async Task<bool> RequestPermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }

            return status == PermissionStatus.Granted;
        }
    }
}