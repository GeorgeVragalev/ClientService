using System.Text;
using ClientService.Helpers;
using ClientService.Models;
using Newtonsoft.Json;

namespace ClientService.Services.RestaurantService;

public class RestaurantService : IRestaurantService
{
    //get menu's from restaurants
    public static IList<RestaurantData>? RestaurantData = null;

    public async Task<IList<RestaurantData>?> GetRestaurantsData()
    {
        if (RestaurantData != null)
        {
            return RestaurantData;
        }
        
        try
        {
            var url = Settings.Settings.GlovoUrl+"/menu";
            using var client = new HttpClient();
            IList<RestaurantData>? menus = null;
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            { 
                PrintConsole.Write($"Got menus from glovo", ConsoleColor.Green);
                menus = await response.Content.ReadFromJsonAsync<IList<RestaurantData>>();
                RestaurantData = menus;
            }
            
            return menus;
        }
        catch (Exception e)
        {
            PrintConsole.Write($"Failed to get menus", ConsoleColor.DarkRed);
            throw;
        }
    }
}