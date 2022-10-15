using ClientService.ClientService;
using ClientService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers;

[ApiController]
[Route("/client")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public ContentResult Connect()
    {
        return Content("Hi");
    }
    
    //recieve order from food ordering service and send it to client
    [HttpPost]
    public void Distribution([FromBody] GroupOrder order)
    {
        Console.WriteLine("Order "+ order.Id+" received");
        _clientService.ServeOrder(order);
    }
}