using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.DAL.DBContext;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class UserServices
{
    private readonly AuthDBContext _context;

    public UserServices(AuthDBContext context)
    {
        _context = context;
    }

    public async Task<TokenDTO> LoginUser(LoginCredentialDTO model)
    {
        var identity = await GetIdentity(model.Email, model.Password);

        if (identity == null)
        {
            throw new NullReferenceException(message: "Login or password failed");
        }

        return new TokenDTO
        {
            AccessToken = ""
        };
    }
    
    private async Task<ClaimsIdentity?> GetIdentity(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
        
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
            };
    
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
    
            return claimsIdentity;
        }
    
        return null;
    }
    
    // private string GetEncodeJwtToken(ClaimsIdentity? identity)
    // {
    //     var now = DateTime.UtcNow;
    //     //создаем JWT токен
    //     var jwt = new JwtSecurityToken(
    //         issuer: JwtConfigurations.Issuer,
    //         audience: JwtConfigurations.Audience,
    //         notBefore: now,
    //         claims: identity?.Claims,
    //         expires: now.AddMinutes(JwtConfigurations.Lifetime),
    //         signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    //
    //     return new JwtSecurityTokenHandler().WriteToken(jwt);
    // }
}