using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FoodNearMe.Models
{
    /// <summary>
    /// Třída obsahující GPS souřadnice a informace o jejich původu
    /// </summary>
    public class Gps
    {
        public static explicit operator Position(Gps gps)
        {
            return new Position(gps.Latitude, gps.Longitude);
        }

        /// <summary>
        /// Zeměpisná šířka
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Zeměpisná délka
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Vypíše vlastnosti třídy
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Latitude.ToString(string.Empty, CultureInfo.InvariantCulture)},{Longitude.ToString(string.Empty, CultureInfo.InvariantCulture)}";
        }

        public override bool Equals(object other)
        {
            return double.Equals(Latitude, (other as Gps).Latitude) && double.Equals(Longitude, (other as Gps).Longitude);
        }
    }
}
