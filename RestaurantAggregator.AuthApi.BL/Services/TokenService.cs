using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RestaurantAggregator.AuthApi.Common.IServices;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class TokenService: ITokenService
{
    // private readonly IConfiguration _configuration;
    //
    // public TokenService(IConfiguration configuration)
    // {
    //     _configuration = configuration;
    // }

    // public string CreateToken(ApplicationUser user, List<IdentityRole<long>> roles)
    // {
    //     var token = user
    //         .CreateClaims(roles)
    //         .CreateJwtToken(_configuration);
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     
    //     return tokenHandler.WriteToken(token);
    // }
}