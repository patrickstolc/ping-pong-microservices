using Moq;
using PingService.Controllers;
using SharedMessages;

namespace PingService.Tests;

public class TestPingServicePost
{
    [Fact]
    public void PostShould_CallSend()
    {
        // Arrange
        var mock = new Mock<MessageClient>();
        mock.Setup(
            client => client.Send(
                new PingMessage{ Message = "Ping!" },
                "ping-message"
            )
        );
        
        var controller = new PingController(
            mock.Object
        );
        
        // Act
        var result = controller.Post();

        // Assert
        Assert.True(result);
        
        mock.Verify(
            client => client.Send(
                new PingMessage{ Message = "Ping!" },
                "ping-message"
            ),
            Times.AtMostOnce
        );
    }
}