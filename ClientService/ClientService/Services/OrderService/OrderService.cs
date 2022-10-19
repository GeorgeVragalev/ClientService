using System.Text;
using ClientService.Helpers;
using ClientService.Models;
using ClientService.Models.Enum;
using ClientService.Services.RestaurantService;
using Newtonsoft.Json;

namespace ClientService.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantService _restaurantService;
    private static Mutex _mutex = new();

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

            var url = Settings.Settings.GlovoUrl+"/order";
            using var client = new HttpClient();
            
            PrintConsole.Write($"Group Order {order.Id} with {order.Orders.Count} orders sent to glovo", ConsoleColor.Green);
            await client.PostAsync(url, data);
        }
        catch (Exception e)
        {
            PrintConsole.Write(Thread.CurrentThread.Name + " Failed to send order id: " + order.Id,
                ConsoleColor.DarkRed);
        }
    }

    //randomnly generate order from given menus
    public async Task<GroupOrder> GenerateGroupOrder(int clientId)
    {
        //get menu's and generate random orders from 2 restaurants
        var restaurantData = await _restaurantService.GetRestaurantsDummyData();

        //order from ranmdom number of restuarnts
        if (restaurantData != null)
        {
            _mutex.WaitOne();
            var groupOrder = new GroupOrder()
            {
                Id = IdGenerator.GenerateGroupOrderId(),
                ClientId = clientId,
                Orders = new List<Order>()
            };
            _mutex.ReleaseMutex();
            var restaurantsToOrderFrom = Random.Shared.Next(0, restaurantData.Count);
            
            for (int i = 0; i <= restaurantsToOrderFrom; i++)
            {
                var restaurant = restaurantData[i];
                var order = await GenerateOrder(restaurant, groupOrder.ClientId, groupOrder.Id);
                order.ClientId = clientId;

                groupOrder.Orders.Add(order);
            }

            return await Task.FromResult(groupOrder);
        }

        throw new Exception("No restaurants registered");
    }

    private async Task<Order> GenerateOrder(RestaurantData restaurantData, int clientId, int groupOrderId)
    {
        var foodList = await GenerateOrderFood(restaurantData.Menu);
        var order = new Order
        {
            Id = IdGenerator.GenerateOrderId(),
            Priority = RandomGenerator.NumberGenerator(5),
            PickUpTime = DateTime.Now,
            Foods = foodList.Select(f=>f.Id).ToList(),
            MaxWait = CalculateMaxWaitingTime(foodList),
            ClientId = clientId,
            GroupOrderId = groupOrderId,
            RestaurantId = restaurantData.Id,
            OrderStatusEnum = OrderStatusEnum.IsCooking
        };
        PrintConsole.Write($"Generated order: {order.Id} waiting time {order.MaxWait}", ConsoleColor.Cyan);

        return await Task.FromResult(order);
    }
    
    private async Task<IList<Food>> GenerateOrderFood(IList<Food> menu)
    {
        var size = RandomGenerator.NumberGenerator(5);
        var orderFoodList = new List<Food>();

        for (var id = 0; id < size; id++)
        {
            var randomFood = menu[RandomGenerator.IndexGenerator(menu.Count)];
            orderFoodList.Add(randomFood);
        }

        return await Task.FromResult(orderFoodList);
    }
    
    private static int CalculateMaxWaitingTime(IList<Food> foodList)
    {
        var maxWaitingTime = 0;

        foreach (var food in foodList)
        {
            maxWaitingTime += food.PreparationTime;
        }

        return (int) Math.Ceiling(maxWaitingTime * 1.3);
    }
}