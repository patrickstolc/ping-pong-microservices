using EasyNetQ;
using SharedMessages;

namespace PingService;

public class MessageHandler : BackgroundService
{
    private readonly Settings _settings;
    public MessageHandler(Settings settings)
    {
        _settings = settings;
    }
    private void HandlePongMessage(PongMessage message)
    {
        Console.WriteLine($"Got pong: {message.Message}");
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Message handler is running..");
        
        var connectionString = _settings.ConnectionStrings?["RabbitMQ"].ConnectionString;
        if(connectionString == null)
        {
            throw new Exception("RabbitMQ connection string not found in settings");
        }

        var messageClient = new MessageClient(
            RabbitHutch.CreateBus(connectionString)    
        );
        
        messageClient.Listen<PongMessage>(HandlePongMessage, "pong-message");
        
        while(!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
        Console.WriteLine("Message handler is stopping..");
    }
}