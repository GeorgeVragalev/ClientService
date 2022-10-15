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

    public async Task<IList<RestaurantData>?> GetRestaurantsDummyData()
    {
        var restaurants = new List<RestaurantData>()
        {
            new RestaurantData()
            {
                Id = 1,
                Menu = new List<Food>()
                {
                    new Food
                    {
                        Id = 1,
                        Name = "Pizza",
                        PreparationTime = 20
                    },
                    new Food
                    {
                        Id = 2,
                        Name = "Salad",
                        PreparationTime = 10
                    },
                    new Food
                    {
                        Id = 3,
                        Name = " Zeama",
                        PreparationTime = 7
                    },
                },
                Rating = 4,
                RestaurantName = "Andy's Pizza"
            },
            new RestaurantData()
            {
                Id = 2,
                Menu = new List<Food>()
                {
                    new Food
                    {
                        Id = 4,
                        Name = "Scallop Sashimi with Meyer Lemon Confit",
                        PreparationTime = 32
                    },
                    new Food
                    {
                        Id = 5,
                        Name = "Island Duck with Mulberry Mustard",
                        PreparationTime = 35
                    },
                    new Food
                    {
                        Id = 6,
                        Name = "Waffles",
                        PreparationTime = 10
                    },
                    new Food
                    {
                        Id = 7,
                        Name = "Aubergine",
                        PreparationTime = 20
                    }
                },
                Rating = 4.8,
                RestaurantName = "Pegas"
            }
        };

        return await Task.FromResult<IList<RestaurantData>?>(restaurants);
    }
}