using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodNearMe.Models
{
    public class CustomPin
    {
        public Gps Location { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Color Color { get; set; }
    }
}
