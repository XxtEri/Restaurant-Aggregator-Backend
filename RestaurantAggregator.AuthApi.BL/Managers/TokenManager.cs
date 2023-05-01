using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using RestaurantAggregator.CommonFiles;

namespace RestaurantAggregator.AuthApi.BL.Managers;

public class TokenManager
{
    public static string CreateAccessToken(List<Claim> claims)
    {
        var nowTime = DateTime.Now;

        var jwt = new JwtSecurityToken(
            issuer: JwtConfigs.Issuer,
            audience: JwtConfigs.Audience,
            notBefore: nowTime,
            claims: claims,
            expires: nowTime.AddMinutes(JwtConfigs.Lifetime),
            signingCredentials: new SigningCredentials(JwtConfigs.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    public static string CreateRefreshToken(List<Claim> claims)
    {
        var number = new byte[64];
        using var randomNumber = RandomNumberGenerator.Create();
        randomNumber.GetBytes(number);
        
        return Convert.ToBase64String(number);
    }

    public static string? GetIdOldToken(string oldJwtToken)
    {
        var tokenValidParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtConfigs.Issuer,
            ValidAudience = JwtConfigs.Audience,
            IssuerSigningKey = JwtConfigs.GetSymmetricSecurityKey()
        };

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(oldJwtToken, tokenValidParams, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken token || !token.Header.Alg.Equals(SecurityAlgorithms.Sha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}