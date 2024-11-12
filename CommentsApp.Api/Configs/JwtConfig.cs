using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommentsApp.Api.Core.Extensions;
using CommentsApp.Core.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace CommentsApp.Api.Configs;

public class JwtConfig(IConfiguration configuration)
{
    public string Audience { get; } = configuration.GetValueOrThrow("Jwt:Audience");
    public string Issuer { get; } = configuration.GetValueOrThrow("Jwt:Issuer");
    public TimeSpan Lifetime { get; } = TimeSpan.FromMinutes(configuration.GetValueOrThrow<long>("Jwt:Lifetime"));

    private readonly byte[] _key = CryptoHelper.GetPbkdf2Bytes(configuration.GetValueOrThrow("Jwt:Secret"));

    private SecurityKey SecurityKey => new SymmetricSecurityKey(_key);


    public TokenValidationParameters TokenValidationParameters => new()
    {
        ValidAudience = Audience,
        ValidateAudience = true,
        //
        ValidIssuer = Issuer,
        ValidateIssuer = true,
        //
        ValidateLifetime = true,
        LifetimeValidator = (before, expires, token, parameters) =>
        {
            if (expires is not { } to) return false;
            return to.CompareTo(DateTime.UtcNow.AddMinutes(1)) <= 0;
        },
        IssuerSigningKey = SecurityKey
    };

    public string GenerateJwtToken(Guid userId, string refreshToken)
    {
        var token = new JwtSecurityToken(Issuer, Audience,
            [new Claim("user_id", userId.ToString()),
            new Claim("refresh", refreshToken)],
            expires: DateTime.UtcNow.Add(Lifetime),
            signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}