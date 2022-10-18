using ClientService.Models;

namespace ClientService.ClientService;

public interface IClientService
{
    public Task ExecuteCode(CancellationToken cancellationToken);
    public Task ServeOrder(GroupOrder groupOrder);
}