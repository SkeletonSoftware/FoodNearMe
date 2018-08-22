using FoodNearMe.Models;
using System.Collections.ObjectModel;

namespace FoodNearMe.Controls
{
    public class CustomMap : Xamarin.Forms.Maps.Map
    {
        private ObservableCollection<CustomPin> customPins;

        public CustomMap()
        {
            customPins = new ObservableCollection<CustomPin>();
        }

        public ObservableCollection<CustomPin> CustomPins
        {
            get { return this.customPins; }
        }
    }
}
