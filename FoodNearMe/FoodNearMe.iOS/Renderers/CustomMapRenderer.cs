using CoreLocation;
using FoodNearMe.Models;
using MapKit;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Collections.ObjectModel;
using CoreGraphics;
using System.Drawing;
using FoodNearMe.Controls;

[assembly: ExportRenderer(typeof(CustomMap), typeof(FoodNearMe.iOS.Renderers.CustomMapRenderer))]
namespace FoodNearMe.iOS.Renderers
{
    public class CustomMapRenderer : Xamarin.Forms.Maps.iOS.MapRenderer
    {
        private ObservableCollection<CustomPin> customPins;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                ((CustomMap)e.OldElement).CustomPins.CollectionChanged -= FormsMap_PinsUpdated;

                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.GetViewForAnnotation = null;
                }
            }

            if (e.NewElement != null)
            {
                ((CustomMap)e.NewElement).CustomPins.CollectionChanged += FormsMap_PinsUpdated;

                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                }
            }
        }

        private void FormsMap_PinsUpdated(object sender, EventArgs e)
        {
            var nativeMap = Control as MKMapView;
            if (nativeMap != null)
            {
                customPins = ((CustomMap)Element).CustomPins;
                foreach (var pin in customPins)
                {
                    var annotation = new ColorPointAnnotation(pin);
                    nativeMap.AddAnnotation(annotation);
                }
            }
        }

        private MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            var anno = annotation as MKPointAnnotation;
            var customPin = GetCustomPin(anno);
            if (customPin == null)
                return null;

            annotationView = mapView.DequeueReusableAnnotation("pin");
            if (annotationView == null)
            {
                annotationView = new MKAnnotationView(annotation, "pin");
                ColorPointAnnotation colorPointAnnotation = annotation as ColorPointAnnotation;
                if (colorPointAnnotation != null)
                {
                    annotationView.Image = GetPinImage(colorPointAnnotation.pinColor);
                }
            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }
        
        private CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            if (annotation == null) return null;

            var position = new Gps()
            {
                Latitude = annotation.Coordinate.Latitude,
                Longitude = annotation.Coordinate.Longitude
            };

            foreach (var pin in customPins)
            {
                if (pin.Location.Equals(position))
                {
                    return pin;
                }
            }
            return null;
        }

        /// <summary>
        /// Získá pin dle zadané barvy
        /// barevný podklad vytvoří ze šablony pin.png
        /// černý obrys pin_contour.png
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private UIImage GetPinImage(UIColor color)
        {
            UIImage image = UIImage.FromBundle("pin.png");
            UIImage image2 = UIImage.FromBundle("pin_contour.png");
            UIImage pinImage = null;

            UIGraphics.BeginImageContextWithOptions(image.Size, false, 0.0f);
            using (CGContext context = UIGraphics.GetCurrentContext())
            {
                context.TranslateCTM(0, image.Size.Height / 2);
                context.ScaleCTM(1.0f, -1.0f);

                var rect = new RectangleF(0, 0, (float)image.Size.Width / 2, (float)image.Size.Height / 2);

                context.SetBlendMode(CGBlendMode.Normal);
                context.DrawImage(rect, image.CGImage);

                context.SetBlendMode(CGBlendMode.SourceIn);
                context.SetFillColor(color.CGColor);
                context.FillRect(rect);

                context.SetBlendMode(CGBlendMode.Normal);
                context.DrawImage(rect, image2.CGImage);

                pinImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
            }
            return pinImage;
        }
    }

    sealed class ColorPointAnnotation : MKPointAnnotation
    {
        public UIColor pinColor;

        public ColorPointAnnotation(CustomPin pin)
        {
            SetCoordinate(new CLLocationCoordinate2D(pin.Location.Latitude, pin.Location.Longitude));
            Title = pin.Title;
            Subtitle = pin.Description;
            pinColor = pin.Color.ToUIColor();
            Init();
        }
    }
}
