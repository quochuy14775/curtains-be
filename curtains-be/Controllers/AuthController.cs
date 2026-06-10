using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using curtains_be.Common;
using curtains_be.DTOs.Requests;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace curtains_be.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration config, IAntiforgery antiforgery) : ControllerBase
{
    [HttpGet("csrf-token")]
    public IActionResult GetCsrfToken()
    {
        var token = antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
        return Ok(token);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var validUsername = config["Admin:Username"];
        var validPassword = config["Admin:Password"];

        if (dto.Username != validUsername || dto.Password != validPassword)
            return Unauthorized(ResponseMessage.Fail("Invalid username or password"));

        var token = GenerateJwtToken();

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(config["Jwt:ExpireMinutes"]!))
        });

        return Ok(ResponseMessage.Ok("Logged in successfully"));
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Ok(ResponseMessage.Ok("Logged out successfully"));
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        return Ok(username);
    }

    private string GenerateJwtToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, config["Admin:Username"]!),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(config["Jwt:ExpireMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
