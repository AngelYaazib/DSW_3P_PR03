
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DSW_3P_PR03.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DSW_3P_PR03.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        var usuarioCorrecto =
            _configuration["DemoUser:Username"];

        var passwordCorrecto =
            _configuration["DemoUser:Password"];

        if (string.IsNullOrWhiteSpace(usuarioCorrecto) ||
            string.IsNullOrWhiteSpace(passwordCorrecto))
        {
            return StatusCode(500, new
            {
                mensaje = "El usuario de demostracion no esta configurado."
            });
        }

        if (login.Usuario != usuarioCorrecto ||
            login.Password != passwordCorrecto)
        {
            return Unauthorized(new
            {
                mensaje = "Credenciales incorrectas."
            });
        }

        var jwt = _configuration.GetSection("Jwt");

        var keyBase64 = jwt["Key"]
            ?? throw new InvalidOperationException("Falta Jwt:Key.");

        var key = Convert.FromBase64String(keyBase64);

        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];

        var expirationMinutes = int.Parse(
            jwt["ExpirationMinutes"] ?? "30"
        );

        if (expirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "El tiempo de expiracion debe ser mayor que cero."
            );
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.Usuario),
            new Claim(ClaimTypes.Role, "Administrador")
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        );

        var expiration = DateTime.UtcNow.AddMinutes(
            expirationMinutes
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Ok(new
        {
            access_token = tokenString,
            token_type = "Bearer",
            expires_in = expirationMinutes * 60
        });
    }
}

