using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class AuthController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

  [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        // Validação simples para demonstração - em produção usar sistema real de autenticação
        if (request.Username == "admin" && request.Password == "123456")
        {
            var token = GenerateJwtToken(request.Username);
            return Ok(new LoginResponse(token));
        }

        return Unauthorized("Credenciais inválidas");
    }

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken
        (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public sealed record LoginRequest(string Username, string Password);
public sealed record LoginResponse(string Token);
