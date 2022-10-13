using ClientService.Models;

namespace ClientService.Services.RestaurantService;

public interface IRestaurantService
{
    public Task<IList<RestaurantData>?> GetRestaurantsData();
}