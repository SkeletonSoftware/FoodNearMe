using System;
using System.Threading.Tasks;
using FoodNearMe.Models;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using FoodNearMe.DependencyServices;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using LocationListener = FoodNearMe.Droid.Tools.LocationListener;

[assembly: Dependency(typeof(FoodNearMe.Droid.DependencyServices.Location))]
namespace FoodNearMe.Droid.DependencyServices
{
    public class Location : ILocation
    {
        public async Task<Gps> GetLocation()
        {
            if (await RequestPermissions())
            {
                //Vytvoøení listeneru
                var locationListener = new LocationListener();
                var locationSource = new TaskCompletionSource<Gps>();
                locationListener.SetSource(locationSource);

                using (var apiClient = new GoogleApiClient.Builder(Android.App.Application.Context, locationListener, locationListener).AddApi(LocationServices.API).Build())
                {
                    //Handler který se zavolá když dojde k napojení na API
                    EventHandler handler = async (sender, e) =>
                    {
                        if (apiClient != null && apiClient.IsConnected)
                        {
                            var locationRequest = new LocationRequest();
                            locationRequest.SetMaxWaitTime(10000);
                            locationRequest.SetNumUpdates(1);
                            var availability = LocationServices.FusedLocationApi.GetLocationAvailability(apiClient);
                            if (availability.IsLocationAvailable)
                            {
                                await LocationServices.FusedLocationApi.RequestLocationUpdatesAsync(apiClient, locationRequest, locationListener);
                            }
                            else
                            {
                                locationSource.SetResult(null);
                            }
                        }
                    };

                    //Zahájení komunikace s API
                    locationListener.Connected += handler;
                    apiClient.Connect();

                    //Èekání na výsledek
                    Gps output = null;
                    output = await locationSource.Task;

                    locationListener.Connected -= handler;
                    await this.StopFusedLocation(apiClient, locationListener);
                    return output;
                }
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

        private async Task StopFusedLocation(GoogleApiClient client, LocationListener listener)
        {
            if (client.IsConnected)
            {
                var availability = LocationServices.FusedLocationApi.GetLocationAvailability(client);
                if (availability.IsLocationAvailable)
                {
                    await LocationServices.FusedLocationApi.RemoveLocationUpdatesAsync(client, listener);
                    client.Disconnect();
                }
            }
        }
    }
}