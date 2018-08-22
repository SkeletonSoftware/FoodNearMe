using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Maps.Model;
using FoodNearMe.Controls;

[assembly: ExportRenderer(typeof(FoodNearMe.Controls.CustomMap), typeof(FoodNearMe.Droid.Renderers.CustomMapRenderer))]
namespace FoodNearMe.Droid.Renderers
{
    public class CustomMapRenderer : Xamarin.Forms.Maps.Android.MapRenderer
    {
        private bool isDrawn = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                ((CustomMap)e.OldElement).CustomPins.CollectionChanged -= FormsMap_PinsUpdated;
            }

            if (e.NewElement != null)
            {
                ((CustomMap)e.NewElement).CustomPins.CollectionChanged += FormsMap_PinsUpdated;
            }
        }

        private void FormsMap_PinsUpdated(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.CreateNativePins(this.Element as CustomMap);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(e.PropertyName == nameof(Element.VisibleRegion) && !isDrawn)
            {
                this.CreateNativePins(this.Element as CustomMap);
                isDrawn = true;
            }
        }

        private void CreateNativePins(CustomMap customMap)
        {
            if (NativeMap != null)
            {
                NativeMap.Clear();
                if (customMap.CustomPins?.Count > 0)
                {
                    for (int i = 0; i < customMap.CustomPins.Count; i++)
                    {
                        var marker = new MarkerOptions();
                        marker.SetPosition(new LatLng(customMap.CustomPins[i].Location.Latitude, customMap.CustomPins[i].Location.Longitude));
                        marker.SetTitle(customMap.CustomPins[i].Title);
                        marker.SetSnippet(customMap.CustomPins[i].Description);

                        float[] hsv = new float[3];
                        Android.Graphics.Color.ColorToHSV(customMap.CustomPins[i].Color.ToAndroid(), hsv);
                        var bitmap = BitmapDescriptorFactory.DefaultMarker(hsv[0]);
                        marker.SetIcon(bitmap);
                        NativeMap.AddMarker(marker);
                    }
                }
            }
        }
    }
}