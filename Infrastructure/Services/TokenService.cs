using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tamkeen.Application.Interfaces;
using Tamkeen.Infrastructure.Setting;

public class TokenService : ITokenService
{
    private readonly JwtSetting _jwt;

    public TokenService(IOptions<JwtSetting> jwt)
    {
        _jwt = jwt.Value;
    }
    public string GenerateToken(string userId, string email, IList<string> roles)
    {
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Email, email),
    };

        foreach (var role in roles)
            claims.Add(new Claim("role", role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

        var token = new JwtSecurityToken(
            issuer: "Tamkeen",
            audience: "TamkeenUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(_jwt.DurationInMonths),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}