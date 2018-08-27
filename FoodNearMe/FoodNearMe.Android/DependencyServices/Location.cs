using System.Threading.Tasks;
using FoodNearMe.Models;
using Android.Gms.Location;
using FoodNearMe.DependencyServices;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

[assembly: Dependency(typeof(FoodNearMe.Droid.DependencyServices.Location))]
namespace FoodNearMe.Droid.DependencyServices
{
    public class Location : ILocation
    {
        public async Task<Gps> GetLocation()
        {
            if (await RequestPermissions())
            {
                var fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(CrossCurrentActivity.Current.Activity);

                Android.Locations.Location location = await fusedLocationProviderClient.GetLastLocationAsync();

                Gps output = new Gps();
                output.Latitude = location.Latitude;
                output.Longitude = location.Longitude;

                return output;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> RequestPermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }

            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}