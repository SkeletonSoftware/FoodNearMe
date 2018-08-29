namespace FoodNearMe.Models
{
    enum RestaurantQuality{ Bad, Good, VeryGood, Amazing}

    class Restaurant
    {
        public string DisplayName { get; set; }
        public Gps Location { get; set; }
        public RestaurantQuality Quality { get; set; }
        public string Description { get; set; }
    }
}
