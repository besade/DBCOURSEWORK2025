using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shop.Tests.Integration;

public static class AuthHelpers
{
    private const string Key = "супер_секретний_ключ_мінімум_32_символи";
    private const string Issuer = "ShopAPI";
    private const string Audience = "ShopClients";

    public static string GenerateToken(int userId, string email, bool isAdmin)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, isAdmin ? "Admin" : "User")
        };
        var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        var creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(Issuer, Audience, claims,
            expires: DateTime.UtcNow.AddDays(1), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}