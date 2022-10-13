using ClientService.Models;

namespace ClientService.ClientService;

public interface IClientService
{
    public Task ExecuteCode(CancellationToken cancellationToken);
    public void ServeOrder(GroupOrder groupOrder);
}