namespace AuthService.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

public class AuthorizationResult
{
    public bool Success { get; set; }
    public List<Claim>? Claims { get; set; }
}

public class JwtTokenService
{
    private const string SecurityKey = "mMdAhocQbIAa1/4iD8W5BiDCD9Lxg9ULp4qROgJVN8oRZommyAsnRalnNlzWGbKGJItr/kh2jVd2d9brhSBAJttV7NE47dvyX6n36cFKlnz3k9AodqqVgH/S52oQMYamtI+HsQqBmsvZMqOE+oGlEIzJG9tmDZ1JE/qJHq+bXo3RCEuBf26dGuIG4DWpjh+G4xTVC7ZoByCmq5zTUUyTlFZCQ2483iJe1Thkem9mlzt3cOy8O5SYJBafIb0xdIBYEoHl56Z805fO/W4eAw+M5stSCUdJTBUtWbCiId9zSapmilb20sCg4l5xYTsaJImTfHlo0t9kF1o/RXwr1cw3zCPoyt9tjWhZ83LMsi1ydBg=";

    public AuthenticateResult DecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecurityKey);
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        }, out var validatedToken);
        
        
        if (principal == null)
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        var claims = new List<Claim>();
        foreach(var claim in principal.Claims)
        {
            claims.Add(new Claim(claim.Type, claim.Value));
        }
        var claimsIdentity = new ClaimsIdentity(claims, "dev");
        var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
        var ticket = new AuthenticationTicket(claimPrincipal, "dev");
        return AuthenticateResult.Success(ticket);
    }
    public AuthenticationToken CreateToken()
    {       
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new Claim("scope", "ping.read"),
            new Claim("scope", "ping.post")
        };
        
        var tokenOptions = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: claims
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var authToken = new AuthenticationToken
        {
            Value = tokenString,
        };

        return authToken;
    }
}