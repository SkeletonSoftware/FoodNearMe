using FoodNearMe.Models;
using System.Threading.Tasks;

namespace FoodNearMe.DependencyServices
{
    public interface ILocation
    {
        Task<Gps> GetLocation();

        Task<bool> RequestPermissions();
    }
}
