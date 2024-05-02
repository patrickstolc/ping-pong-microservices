using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PingService;

namespace PongService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FaucetController : ControllerBase
{
    private readonly Settings _settings;

    public FaucetController(Settings settings)
    {
        _settings = settings;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            _settings.JwtIssuer,
            _settings.JwtIssuer,
            null,
            expires: DateTime.Now.AddHours(12),
            signingCredentials: credentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return Ok(token);
    }
}