using FoodNearMe.Models;
using System.Threading.Tasks;

namespace FoodNearMe.Services
{
    public interface ILocation
    {
        Task<Gps> GetLocation();

        Task<bool> RequestPermissions();
    }
}
