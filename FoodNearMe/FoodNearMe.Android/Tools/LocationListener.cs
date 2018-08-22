using System;
using Android.OS;
using Android.Gms.Common.Apis;
using System.Threading.Tasks;
using FoodNearMe.Models;
using Android.Gms.Common;

namespace FoodNearMe.Droid.Tools
{
    class LocationListener : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {
        /// <summary>
        /// Událost která se volá, pokud dojde k pøipojení k GooglePlayAPI
        /// </summary>
        public event EventHandler Connected;

        private void CallConnected()
        {
            if (this.Connected != null)
                this.Connected(this, EventArgs.Empty);
        }

        private TaskCompletionSource<Gps> source;

        public LocationListener()
        {

        }

        public void SetSource(TaskCompletionSource<Gps> source)
        {
            this.source = source;
        }

        public void OnConnected(Bundle connectionHint)
        {
            this.CallConnected();
        }

        public void OnConnectionSuspended(int cause)
        {
            if (this.source != null)
            {
                source.SetCanceled();
                source = null;
            }
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (this.source != null)
            {
                source.SetCanceled();
                source = null;
            }
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            if (this.source != null)
            {
                this.source.SetResult(new Gps()
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                });
                this.source = null;
            }
        }
    }
}