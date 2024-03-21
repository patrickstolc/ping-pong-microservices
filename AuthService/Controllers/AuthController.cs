using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    
    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    public IActionResult Login()
    {
        // any login attempt will be successful
        // pass a user object to this method and validate it if doing the actual implementation
        var token = _jwtTokenService.CreateToken();
        return Ok(token);
    }
}