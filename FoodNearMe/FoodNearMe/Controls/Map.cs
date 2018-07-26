using FoodNearMe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodNearMe.Controls
{
    public class Map : Xamarin.Forms.Maps.Map
    {
        private ObservableCollection<CustomPin> pins;

        public Map()
        {
            pins = new ObservableCollection<CustomPin>();
        }

        public ObservableCollection<CustomPin> CustomPins
        {
            get { return this.pins; }
        }
    }
}
