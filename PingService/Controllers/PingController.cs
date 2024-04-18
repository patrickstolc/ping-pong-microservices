using Microsoft.AspNetCore.Mvc;
using SharedMessages;

namespace PingService.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    private readonly MessageClient _messageClient;
    private readonly ILogger _logger;
    private readonly Settings _settings;
    
    public PingController(MessageClient messageClient, ILoggerFactory loggerFactory, Settings settings)
    {
        _messageClient = messageClient;
        _logger = loggerFactory.CreateLogger<PingController>();
        _settings = settings;
    }

    private string GetPongServiceUrl()
    {
        var url = _settings.Services?["PongService"].Host;
        if(url == null)
        {
            throw new Exception("PongService URL not found in settings");
        }
        return $"{url}/pong";
    }
    
    [HttpGet]
    public async Task<string> Get()
    {
        try {
            var url = GetPongServiceUrl();
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);
            return response;
        } catch (Exception e) {
            return "no pong :(";
        }
    }
    
    [HttpPost]
    public bool Post()
    {
        _logger.LogInformation("Sending ping message...");
        _messageClient.Send(
            new PingMessage { Message = "Ping!" },
            "ping-message"
        );
        return true;
    }
}
