using ClientService.Models;

namespace ClientService.Services.OrderService;

public interface IOrderService
{
    public Task<GroupOrder> GenerateGroupOrder(int clientId);
    public Task SendOrder(GroupOrder order);
    public Task SubmitOrderRating(GroupOrder groupOrder);
}