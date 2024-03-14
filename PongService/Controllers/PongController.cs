using Microsoft.AspNetCore.Mvc;
using PingService;
using SharedMessages;

namespace PongService.Controllers;

[ApiController]
[Route("[controller]")]
public class PongController : ControllerBase
{
    private readonly MessageClient _messageClient;
    
    public PongController(MessageClient messageClient)
    {
        _messageClient = messageClient;
    }
    
    [HttpPost]
    public bool Post()
    {
        _messageClient.Send<PongMessage>(new PongMessage{ Message = "Pong!"}, "pong-message");
        return true;
    }
}