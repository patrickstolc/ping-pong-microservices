using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Gateway.Authentication;

public class CustomRemoteAuthenticationOptions : AuthenticationSchemeOptions
{
    public CustomRemoteAuthenticationOptions()
    {
    }
}

public class CustomRemoteAuthenticationHandler : AuthenticationHandler<CustomRemoteAuthenticationOptions>
{
    private HttpClient _httpClient;
    public CustomRemoteAuthenticationHandler(IOptionsMonitor<CustomRemoteAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _httpClient = new HttpClient();
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // make a request to the remote service to validate the token
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://authservice/Auth");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Request.Headers.Authorization.ToString().Replace("Bearer ", ""));

        var response = await _httpClient.SendAsync(requestMessage);
        var authResult = await response.Content.ReadFromJsonAsync<bool>();

        if (!authResult)
        {
            return AuthenticateResult.Fail("Invalid token");
        }   
        
        // remote service validates the token, but does not provide any user information
        var claims = new[] { new Claim(ClaimTypes.Name, "usernameherepleasereplace") };
        var claimsIdentity = new ClaimsIdentity(claims, "dev");
        var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authenticationTicket = new AuthenticationTicket(claimPrincipal, "dev");
        return AuthenticateResult.Success(authenticationTicket);
    }
}