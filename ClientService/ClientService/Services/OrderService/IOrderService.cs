using ClientService.Models;

namespace ClientService.Services.OrderService;

public interface IOrderService
{
    public Task<GroupOrder> GenerateOrder();
    public Task SendOrder(GroupOrder order);
}