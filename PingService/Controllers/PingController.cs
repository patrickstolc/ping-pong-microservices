using Microsoft.AspNetCore.Mvc;
using SharedMessages;

namespace PingService.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    private readonly MessageClient _messageClient;
    
    public PingController(MessageClient messageClient)
    {
        _messageClient = messageClient;
    }
    
    [HttpGet]
    public async Task<string> Get()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync("http://pongservice/pong");
        return response;
    }
    
    [HttpPost]
    public bool Post()
    {
        _messageClient.Send(
            new PingMessage { Message = "Ping!" },
            "ping-message"
        );
        return true;
    }
}
