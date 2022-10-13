using ClientService.Models;
using ClientService.Services.OrderService;
using ClientService.Services.RestaurantService;

namespace ClientService.ClientService;

public class ClientService : IClientService
{
    private readonly IRestaurantService _restaurantService;
    private readonly IOrderService _orderService;

    public static int CurrentClients = 3;
    public ClientService(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    public async Task ExecuteCode(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            //get the first batch of clients and make orders
            //when the are complete we get another batch
            if (CurrentClients >= 3)
            {
                CurrentClients = 0;
                //get menu's and store in memory
                _restaurantService.GetRestaurantsData();
                
                var taskList = new List<Task>
                {
                    Task.Run(() => GenerateOrder()),
                    Task.Run(() => GenerateOrder()),
                    Task.Run(() => GenerateOrder()),
                };

                await Task.WhenAll(taskList);
            }
            Thread.Sleep(2000*Settings.Settings.TimeUnit);
        }
    }

    public void ServeOrder(GroupOrder groupOrder)
    {
        CurrentClients += 1;
        //Get rating and return response back to food service
        
        //todo refactor dining hall and kitchen to accept type of order (hall/from client side)
    }

    private async Task GenerateOrder()
    {
        var order = await _orderService.GenerateOrder();
        await _orderService.SendOrder(order);
    }
}