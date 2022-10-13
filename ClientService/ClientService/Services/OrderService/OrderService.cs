using System.Text;
using ClientService.Helpers;
using ClientService.Models;
using ClientService.Services.RestaurantService;
using Newtonsoft.Json;

namespace ClientService.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantService _restaurantService;

    public OrderService(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    public async Task SendOrder(GroupOrder order)
    {
        try
        {
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = Settings.Settings.GlovoUrl;
            using var client = new HttpClient();

            await client.PostAsync(url, data);
            PrintConsole.Write($"Order {order.Id} with {order.GroupedOrder.Count} orders sent to glovo", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            PrintConsole.Write(Thread.CurrentThread.Name + " Failed to send order id: " + order.Id,
                ConsoleColor.DarkRed);
        }
    }

    //randomnly generate order from given menus
    public async Task<GroupOrder> GenerateOrder()
    {
        //get menu's and generate random orders from 2 restaurants
        var restaurantData = _restaurantService.GetRestaurantsData();
        
        var order = new GroupOrder
        {
            Id = IdGenerator.GenerateId(),
        };
        PrintConsole.Write($"Generated order: {order.Id} waiting time", ConsoleColor.Cyan);

        return await Task.FromResult(order);
    }
}