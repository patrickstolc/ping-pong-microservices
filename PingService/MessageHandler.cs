using EasyNetQ;
using SharedMessages;

namespace PingService;

public class MessageHandler : BackgroundService
{
    private void HandlePongMessage(PongMessage message)
    {
        Console.WriteLine($"Got pong: {message.Message}");
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Message handler is running..");

        var messageClient = new MessageClient(
            RabbitHutch.CreateBus("host=rabbitmq;port=5672;virtualHost=/;username=guest;password=guest")    
        );
        
        messageClient.Listen<PongMessage>(HandlePongMessage, "pong-message");
        
        while(!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
        Console.WriteLine("Message handler is stopping..");
    }
}