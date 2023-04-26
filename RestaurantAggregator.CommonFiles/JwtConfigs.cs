using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RestaurantAggregator.CommonFiles;

public class JwtConfigs
{
    private const string Key = "JWT:SecretKey";
        
    public const string Issuer = "JwtIssuer";
    public const string Audience = "JwtAudience";
    public const int Lifetime = 10;
    public const int RefreshTime = 1;

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}