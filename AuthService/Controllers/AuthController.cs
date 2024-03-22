using System.Security.Claims;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication;
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
    
    [HttpGet]
    public bool Authorize()
    {
        // checking for a valid token in the Authorization header
        var re = Request;

        if (!re.Headers.ContainsKey("Authorization"))
            return false;

        if (!re.Headers["Authorization"].ToString().StartsWith("Bearer "))
            return false;

        var token = re.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        // decode the token & check if it's valid
        var result = _jwtTokenService.DecodeToken(token);
        return result.Succeeded;
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