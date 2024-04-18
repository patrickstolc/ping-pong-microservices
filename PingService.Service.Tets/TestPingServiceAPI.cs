using Moq;
using PingService.Controllers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace PingService.Service.Tets;

public class TestPingServiceAPI : IAsyncLifetime
{
    private WireMockServer _pongService;
    
    public async Task InitializeAsync()
    {
        Environment.SetEnvironmentVariable("PONG_SERVICE_URL", "http://localhost:8080");
        _pongService = WireMockServer.Start(
            new WireMockServerSettings
            {
                Urls = new[] { "http://localhost:8080" }
            }
        );

        _pongService.Given(
            Request.Create().WithPath("/pong").UsingGet()    
        ).RespondWith(
            Response.Create().WithBody("Pong!")    
        );
    }

    public async Task DisposeAsync()
    {
        _pongService.Stop();
    }
    
    [Fact]
    public async void TestGet_ReturnsPong()
    {
        // arrange
        var mock = new Mock<MessageClient>();
        var controller = new PingController(mock.Object);

        // act
        var result = await controller.Get();

        // assert
        Assert.Equal("Pong!", result);
    }
}