using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Animate_backend.Models.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Animate_backend.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthResponse GenerateToken(string email)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, email) };

        var jwt = new JwtSecurityToken(
            issuer:_configuration.GetConnectionString("JwtOptions"),
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.RefreshInterval.Minutes)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Audience.)), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new AuthResponse
        {
            AccessToken = encodedJwt,
            RefreshToken = encodedJwt
        };
    }
}