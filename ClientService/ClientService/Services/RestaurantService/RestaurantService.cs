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
        //todo if we don't have menus here then call glovo and store the menus here
        //call get request from glovo and get all menus and then return them back
        
        try
        {
            var url = Settings.Settings.GlovoUrl;
            using var client = new HttpClient();

            var menus = await client.GetAsync(url);
            PrintConsole.Write($"Got menus from glovo", ConsoleColor.Green);
            RestaurantData = JsonConvert.DeserializeObject<IList<RestaurantData>>(url);
            // var json = JsonConvert.DeserializeObject();
        }
        catch (Exception e)
        {
            PrintConsole.Write($"Failed to get menus", ConsoleColor.DarkRed);
            throw;
        }

        return RestaurantData;
    }
}