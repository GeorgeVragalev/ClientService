using ClientService.Helpers;
using ClientService.Models;
using ClientService.Services.OrderService;
using ClientService.Services.RestaurantService;

namespace ClientService.ClientService;

public class ClientService : IClientService
{
    private readonly IOrderService _orderService;

    private static int CurrentClients = 3;
    private static Mutex _mutex = new();

    public ClientService(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task ExecuteCode(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            //get the first batch of clients and make orders
            //when the are complete we get another batch
            if (CurrentClients >= 0)
            {
                CurrentClients -= 3;
                var clientId1 = IdGenerator.GenerateClientId();
                var clientId2 = IdGenerator.GenerateClientId();
                var clientId3 = IdGenerator.GenerateClientId();
                var client1 = Task.Run(() => GenerateOrder(clientId1), cancellationToken);
                var client2 = Task.Run(() => GenerateOrder(clientId2), cancellationToken);
                var client3 = Task.Run(() => GenerateOrder(clientId3), cancellationToken);

                var taskList = new List<Task>
                {
                    client1, client2, client3
                };

                await Task.WhenAll(taskList);
            }

            Thread.Sleep(20000 * Settings.Settings.TimeUnit);
        }
    }

    public Task ServeOrder(GroupOrder groupOrder)
    {
        _mutex.WaitOne();
        CurrentClients += 1;
        _mutex.ReleaseMutex();

        //Get rating and return response back to food service
        PrintConsole.Write($"Current clients :{CurrentClients}", ConsoleColor.Magenta);
        _orderService.SubmitOrderRating(groupOrder);
        
        return Task.CompletedTask;
    }

    private async Task GenerateOrder(int clientId)
    {
        var order = await _orderService.GenerateGroupOrder(clientId);
        await _orderService.SendOrder(order);
    }
}